using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPG
{ 

    public class TweenRot : TweenBase
    {
        public Vector3 mStartRot;
        public Vector3 mEndRot = new Vector3(1, 0, 0);
        private Vector3 mDistance = Vector3.zero;

        public Transform mTarget;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Play()
        {
            OnPlay();

            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartRot, mEndRot);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndRot - mStartRot;
        }

        public override void ReversePlay()
        {
            OnReversePlay();

            if (mSpeed > 0)
            {
                var dis = Vector3.Distance(mStartRot, mEndRot);
                mDuration = dis * mSpeed;
            }

            mDistance = mEndRot - mStartRot;
        }

        protected override void OnUpdate(float value)
        {
            if (mTarget == null)
            {
                mTarget = transform;
            }
            mTarget.rotation = Quaternion.Euler(mStartRot + mDistance * value);
        }

        //public override void Copy(TweenBase tweenBase)
        //{
        //    base.Copy(tweenBase);
        //    var tweenMove = tweenBase as TweenMove;
        //    mStartPos = tweenMove.mStartPos;
        //    mEndPos = tweenMove.mEndPos;
        //    mDistance = tweenMove.mDistance;
        //    m_space = tweenMove.m_space;
        //}

        //[ContextMenu("From")]
        //void FormPos()
        //{
        //    if (m_space == Space.Self)
        //        mStartPos = mTarget.localPosition;
        //    else
        //        mStartPos = mTarget.position;
        //}

        //[ContextMenu("To")]
        //void ToPos()
        //{
        //    if (m_space == Space.Self)
        //        mEndPos = mTarget.localPosition;
        //    else
        //        mEndPos = mTarget.position; ;
        //}
    }

}
