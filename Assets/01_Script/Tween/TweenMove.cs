using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRPG
{
    public class TweenMove : TweenBase
    {
        public Vector3 mStartPos;
        public Vector3 mEndPos = new Vector3(1, 0, 0);
        private Vector3 mDistance = Vector3.zero;

        public Transform mTarget;
        public RectTransform m_rtTarget;
        public Space m_space = Space.Self;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Play()
        {
            OnPlay();

            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartPos, mEndPos);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndPos - mStartPos;
        }

        public override void ReversePlay()
        {
            OnReversePlay();

            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartPos, mEndPos);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndPos - mStartPos;
        }

        protected override void OnUpdate(float value)
        {   
            if (mTarget != null)
            {
                if (m_space == Space.Self)
                    mTarget.localPosition = mStartPos + mDistance * value;
                else
                    mTarget.position = mStartPos + mDistance * value;
            }
            else if (m_rtTarget != null)
            {
                if (m_space == Space.Self)
                    m_rtTarget.anchoredPosition = mStartPos + mDistance * value;
                else
                    m_rtTarget.position = mStartPos + mDistance * value;
            }
        }

        public override void Copy(TweenBase tweenBase)
        {
            base.Copy(tweenBase);
            var tweenMove = tweenBase as TweenMove;
            mStartPos = tweenMove.mStartPos;
            mEndPos = tweenMove.mEndPos;
            mDistance = tweenMove.mDistance;
            m_space = tweenMove.m_space;
        }

        [ContextMenu("From")]
        void FormPos()
        {
            if (m_space == Space.Self)
                mStartPos = mTarget.localPosition;
            else
                mStartPos = mTarget.position;
        }

        [ContextMenu("To")]
        void ToPos()
        {
            if (m_space == Space.Self)
                mEndPos = mTarget.localPosition;
            else
                mEndPos = mTarget.position; ;
        }
    }

}