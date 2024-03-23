using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NL.Core;

namespace NL.Game.UI
{

    public struct AmmoCounterUIData : IUIData
    {
        public AmmoCount AmmoCount;
    }

    public class UIAmmoCounter : BaseUI, IUIWithData
    {
        public static string UI_PATH = "UI/UI_AmmoCounter";
        public TextMeshProUGUI AmmoCounterText;
        public void UpdateUI(IUIData uIData)
        {
            AmmoCounterUIData ammoCounterUIData = (AmmoCounterUIData)uIData;
            AmmoCount ammoCount = ammoCounterUIData.AmmoCount;

            AmmoCounterText.text = $"{ammoCount.CurrentCount} / {ammoCount.MaxCount}";
        }
    }
}
