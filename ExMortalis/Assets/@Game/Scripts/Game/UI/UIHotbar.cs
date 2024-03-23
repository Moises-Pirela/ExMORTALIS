using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NL.ExMORTALIS.UI;
using UnityEngine;

namespace NL.ExMORTALIS
{
    public class UIHotbar : MonoBehaviour
    {
        public GameObject Hotbar;

        public HotbarSlot[] HotbarSlots;

        void Start()
        {
            OnSelectNewEquipment(0);
        }

        public void OnSelectNewEquipment(int selectedIndex)
        {
            Hotbar.transform.DOMoveY(0, 1f).SetEase(Ease.InOutSine).onComplete += () => 
            {
                HotbarSlots[selectedIndex].OnSelect();
                Hotbar.transform.DOMoveY(-152f, 1f).SetEase(Ease.InOutSine).SetDelay(2f);
            };
        }
    }
}
