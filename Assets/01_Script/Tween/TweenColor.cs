using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MRPG
{
    public class TweenColor : TweenBase
    {
        // private new float mSpeed;

        public SpriteRenderer mSpriteRenderer;
        public Image mImage;
        public RawImage mRawImage;
        public TMP_Text m_text;

        public Color mStartColor = Color.white;
        public Color mEndColor = Color.red;
        private Color mDistance = Color.white;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Play()
        {
            OnPlay();
            mDistance = mEndColor - mStartColor;
        }

        public override void ReversePlay()
        {
            OnReversePlay();
            mDistance = mEndColor - mStartColor;
        }

        protected override void OnUpdate(float value)
        {
            Color color = Color.Lerp(mStartColor, mEndColor, value);
            if (mSpriteRenderer != null)
            {
                mSpriteRenderer.color = color;
            }
            if (mImage != null)
            {
                mImage.color = color;
            }
            if (m_text != null)
            {
                m_text.color = color;
            }
            if (mRawImage != null)
            {
                mRawImage.color = color;
            }
        }
    }
}