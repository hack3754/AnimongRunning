using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPG
{
    public class TweenScale : TweenBase
    {
        public Vector3 mStartScale;
        public Vector3 mEndScale = Vector3.one;
        public Transform mTarget;

        private Vector3 mDistance = new Vector3(0, 0, 0);

        protected override void Awake()
        {
            base.Awake();
        }
        
        public override void Play()
        {
            OnPlay();
            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartScale, mEndScale);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndScale - mStartScale;
        }

        public override void ReversePlay()
        {
            OnReversePlay();
            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartScale, mEndScale);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndScale - mStartScale;
        }

        protected override void OnUpdate(float value)
        {
            if (mTarget == null)
            {
                mTarget = transform;
            }

            mTarget.localScale = mStartScale + mDistance * value;
        }

        public override void Copy(TweenBase tweenBase)
        {
            base.Copy(tweenBase);
            var tweenScae = tweenBase as TweenScale;
            mStartScale = tweenScae.mStartScale;
            mEndScale = tweenScae.mEndScale;
            mDistance = tweenScae.mDistance;
        }

        public void ResetToBeginning()
        {
            mTarget.transform.localScale = mStartScale;
        }
    }
}