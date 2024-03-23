using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace NL.ExMORTALIS.UI
{
    public class UIManager
    {
        private List<IUI> UIs = new List<IUI>();
        private Dictionary<UICommand, Action<IUIData>> UpdateCommands = new Dictionary<UICommand, Action<IUIData>>();
        private Dictionary<UICommand, Action> UICommands = new Dictionary<UICommand, Action>();

        public UIManager()
        {
            RegisterEntityHealthBar();
            RegisterAmmoCounterUI();
            RegisterTabMenuUI();
        }

        public void RegisterEntityHealthBar()
        {
            GameObject entityHealthBarPrefab = GameObject.Instantiate(Resources.Load(UIEntityHealthBar.UI_PATH) as GameObject);
            UIEntityHealthBar uIEntityHealthBar = entityHealthBarPrefab.GetComponent<UIEntityHealthBar>();

            RegisterUI(uIEntityHealthBar);
            RegisterCommand(UICommand.EntityHealthBarUpdate, uIEntityHealthBar.UpdateUI);
            RegisterCommand(UICommand.EntityHealthBarShow, uIEntityHealthBar.Show);
            RegisterCommand(UICommand.EntityHealthBarHide, uIEntityHealthBar.Hide);

            uIEntityHealthBar.Initialize(false);
        }

        public void RegisterAmmoCounterUI()
        {
            GameObject ammoCounterPrefab = GameObject.Instantiate(Resources.Load(UIAmmoCounter.UI_PATH) as GameObject);
            UIAmmoCounter uiAmmoCounter = ammoCounterPrefab.GetComponent<UIAmmoCounter>();

            RegisterUI(uiAmmoCounter);
            RegisterCommand(UICommand.AmmoCounterUpdate, uiAmmoCounter.UpdateUI);
            RegisterCommand(UICommand.AmmoCounterShow, uiAmmoCounter.Show);
            RegisterCommand(UICommand.AmmoCounterHide, uiAmmoCounter.Hide);

            uiAmmoCounter.Initialize(false);
        }

        public void RegisterTabMenuUI()
        {
            GameObject tabMenuPrefab = GameObject.Instantiate(Resources.Load(UITabMenu.UI_PATH) as GameObject);
            UITabMenu uITabMenu = tabMenuPrefab.GetComponent<UITabMenu>();

            RegisterUI(uITabMenu);
            RegisterCommand(UICommand.TabMenuUpdate, uITabMenu.UpdateUI);
            RegisterCommand(UICommand.TabMenuShow, uITabMenu.Show);
            RegisterCommand(UICommand.TabMenuHide, uITabMenu.Hide);

            uITabMenu.Initialize(false);
        }

        public void RegisterUI(IUI ui)
        {
            UIs.Add(ui);
        }

        public void RegisterCommand(UICommand command, Action<IUIData> func)
        {
            UpdateCommands.TryAdd(command, func);
        }

        public void RegisterCommand(UICommand command, Action func)
        {
            UICommands.TryAdd(command, func);
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

        public void SendCommand(UICommand command)
        {
            UICommands[command]();
        }
    }
}
