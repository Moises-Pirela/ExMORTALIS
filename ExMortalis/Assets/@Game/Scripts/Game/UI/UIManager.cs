using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Transendence.Game.UI
{
    public class UIManager
    {
        private List<IUI> UIs = new List<IUI>();
        private Dictionary<UICommand, Action<IUIData>> UpdateCommands = new Dictionary<UICommand, Action<IUIData>>();

        public UIManager()
        {
            RegisterEntityHealthBar();
            RegisterAmmoCounterUI();
        }

        public void RegisterEntityHealthBar()
        {
            GameObject entityHealthBarPrefab = GameObject.Instantiate(Resources.Load(UIEntityHealthBar.UI_PATH) as GameObject);
            UIEntityHealthBar uIEntityHealthBar = entityHealthBarPrefab.GetComponent<UIEntityHealthBar>();

            RegisterUI(uIEntityHealthBar);
            RegisterCommand(UICommand.EntityHealthBarUpdate, uIEntityHealthBar.UpdateUI);

            uIEntityHealthBar.Initialize(false);
        }

        public void RegisterAmmoCounterUI()
        {
            // GameObject entityHealthBarPrefab = GameObject.Instantiate(Resources.Load(UIEntityHealthBar.UI_PATH) as GameObject);
            // UIEntityHealthBar uIEntityHealthBar = entityHealthBarPrefab.GetComponent<UIEntityHealthBar>();

            // RegisterUI(uIEntityHealthBar);
            // RegisterCommand(UICommand.EntityHealthBarUpdate, uIEntityHealthBar.UpdateUI);

            // uIEntityHealthBar.Initialize();
        }

        public void RegisterUI(IUI ui)
        {
            UIs.Add(ui);
        }

        public void RegisterCommand(UICommand command, Action<IUIData> func)
        {
            UpdateCommands.TryAdd(command, func);
        }

        public void RegisterUIWithData(IUIWithData uIWithData)
        {
            UIs.Add(uIWithData);
        }

        public void UnregisterUI(IUI uI)
        {
            UIs.Remove(uI);
        }

        public void SendUpdateCommand(UICommand command, IUIData data)
        {
            UpdateCommands[command](data);
        }
    }
}
