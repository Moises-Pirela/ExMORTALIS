using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Transendence.Game
{
    public class UIEntityHealthBar : MonoBehaviour
    {
        public Image HealthBarImage;
        public TextMeshProUGUI EntityNameText;

        public void UpdateHealthBar(float currentHealth, float maxHealth, string name)
        {
            HealthBarImage.fillAmount = currentHealth/maxHealth;
            EntityNameText.text = name;
        }
    }
}
