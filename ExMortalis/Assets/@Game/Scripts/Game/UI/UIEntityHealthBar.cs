using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NL.ExMORTALIS.UI
{

    public struct EntityHealthBarUIData : IUIData
    {
        public float CurrentHealth;
        public float MaxHealth;
        public string Name;
    }

    public class UIEntityHealthBar : BaseUI, IUIWithData
    {
        public const string UI_PATH = "UI/UI_EntityHealthBar";
        public Image HealthBarImage;
        public TextMeshProUGUI EntityNameText;

        public UICommand GetUpdateCommand()
        {
            return UICommand.EntityHealthBarUpdate;
        }

        public void UpdateUI(IUIData uIData)
        {
            EntityHealthBarUIData data = (EntityHealthBarUIData)uIData;

            HealthBarImage.fillAmount = data.CurrentHealth / data.MaxHealth;
            EntityNameText.text = data.Name;
        }
    }
}
