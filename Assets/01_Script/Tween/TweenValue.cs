using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPG
{
    public class TweenValue : TweenBase
    {
        public float mFrom;
        public float mTo;
        public float value { get; set; }
        private float mDistance = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Play()
        {
            OnPlay();

            if (mSpeed > 0)
            {
                var dis = mTo - mFrom;
                mDuration = dis * mSpeed;
            }

            mDistance = mFrom - mTo;
        }

        public override void ReversePlay()
        {
            OnReversePlay();
            if (mSpeed > 0)
            {
                var dis = mTo - mFrom;
                mDuration = dis * mSpeed;
            }

            mDistance = mFrom - mTo;
        }

        protected override void OnUpdate(float value)
        {
            value = mFrom + mDistance * value;

        }
    }
}