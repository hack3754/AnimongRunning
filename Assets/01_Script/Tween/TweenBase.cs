using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPG
{
    public abstract class TweenBase : MonoBehaviour
    {
        public AnimationCurve mAnimCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public float mDelayTime = 0;
        public float mDuration = 1;
        public float mSpeed;
        public bool mEnableStart;
        [Tooltip("딜레이가 있을때 초기값을 셋팅")]
        public bool mInitData;
        public TweenType tweenType = TweenType.Once;

        private bool mInit = false;
        private Coroutine mCoroutine;
        private TweenType tweenTypeBackup = TweenType.Once;

        public System.Action m_OnCompleat { get; set; }


        // Unity ==============================================================================
        // Unity
        protected virtual void Awake()
        {
            mInit = true;
        }

        protected void OnEnable()
        {
            if (mEnableStart)
            {
                Play();
            }
        }

        private void OnValidate()
        {
            if (mInit == false)
                return;

#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
                return;
#endif
            // Debug.LogWarning("OnValidate");
            if (tweenTypeBackup != tweenType)
            {
                Play();
            }
        }

        // abstract ==============================================================================
        public abstract void Play();
        public abstract void ReversePlay();
        
        protected abstract void OnUpdate(float value);

        public void Stop()
        {
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
            }
        }

        public void BeginAndStop()
        {
            OnUpdate(0);
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
            }
        }

        // Public ==============================================================================

        public virtual void Copy(TweenBase tweenBase)
        {
            mAnimCurve = new AnimationCurve(tweenBase.mAnimCurve.keys);
            mDelayTime = tweenBase.mDelayTime;
            mDuration = tweenBase.mDuration;
            mDelayTime = tweenBase.mSpeed;
            mEnableStart = tweenBase.mEnableStart;
            tweenType = tweenBase.tweenType;
            mDelayTime = tweenBase.mDelayTime;
        }

        // Event ==============================================================================
        // Event

        // Private ==============================================================================
        // Private

        [ContextMenu("Play")]
        protected void TestPlay()
        {
            Play();
        }


        [ContextMenu("ReversePlay")]
        protected void TestReversePlay()
        {
            ReversePlay();
        }

        protected void OnPlay()
        {
            tweenTypeBackup = tweenType;
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
            }

            mCoroutine = StartCoroutine(CoUpdate());
        }

        protected void OnReversePlay()
        {
            tweenTypeBackup = tweenType;
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
            }

            mCoroutine = StartCoroutine(CoUpdate(true));
        }

        private IEnumerator CoUpdate(bool reverse = false)
        {
            Vector3 scale = Vector3.zero;
            float progress = 0;

            if (mDelayTime > 0)
            {
                if (mInitData)
                {
                    OnUpdate(mAnimCurve.Evaluate(0));
                }
                yield return new WaitForSeconds(mDelayTime);
            }

            if (tweenType == TweenType.Once || tweenType == TweenType.Loop)
            {
                while (progress < mDuration)
                {
                    if (this.enabled == false)
                    {
                        OnUpdate(mAnimCurve.Evaluate(1));
                        yield break;
                    }

                    progress += Time.deltaTime;

                    if (progress > mDuration)
                    {
                        if (tweenType == TweenType.Loop)
                        {
                            progress = mDuration - progress;
                        }
                        else
                        {
                            progress = mDuration;
                        }
                    }

                    float value = mAnimCurve.Evaluate(reverse == false ? progress / mDuration : (mDuration - progress) / mDuration);
                    OnUpdate(value);

                    yield return 0;
                }

                OnUpdate(mAnimCurve.Evaluate(1));
            }
            else if (tweenType == TweenType.PingPong)
            {
                while (true)
                {
                    while (progress < mDuration)
                    {
                        if (this.enabled == false)
                        {
                            yield break;
                        }

                        progress += Time.deltaTime;
                        if (progress > mDuration)
                        {
                            break;
                        }

                        float value = mAnimCurve.Evaluate(progress / mDuration);
                        OnUpdate(value);
                        yield return 0;
                    }

                    progress = mDuration - (progress - mDuration);

                    while (progress > 0)
                    {
                        if (this.enabled == false)
                        {
                            yield break;
                        }

                        progress -= Time.deltaTime;
                        if (progress < 0)
                        {
                            break;
                        }

                        float value = mAnimCurve.Evaluate(progress / mDuration);
                        OnUpdate(value);
                        yield return 0;
                    }
                }
            }

            m_OnCompleat?.Invoke();
            mCoroutine = null;
        }

        // protected ==============================================================================
        // protected


    }
}