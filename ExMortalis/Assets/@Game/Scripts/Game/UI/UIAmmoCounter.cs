using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Transendence.Core;

namespace Transendence.Game.UI
{

    public class UIAmmoCounter : MonoBehaviour
    {
        public TextMeshProUGUI AmmoCounterText;

        public void UpdateText(AmmoCount currentAmmoCount)
        {
            AmmoCounterText.text = $"{currentAmmoCount.CurrentCount} / {currentAmmoCount.MaxCount}";
        }
    }
}
