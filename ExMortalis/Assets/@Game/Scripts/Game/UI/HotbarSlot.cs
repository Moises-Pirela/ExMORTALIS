using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NL.ExMORTALIS.UI
{
    public class HotbarSlot : MonoBehaviour
    {
        public Image HighlightImage;

        void Awake()
        {
            HighlightImage.DOFade(0,0f);
        }

        public void OnSelect()
        {
            HighlightImage.DOFade(1, 1f).onComplete += () => { HighlightImage.DOFade(0, 1f); };
        }
    }
}
