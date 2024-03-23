using System;
using System.Collections.Generic;
using NL.Core.Configs;
using NL.Core.Postprocess;
using NL.Utilities;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.PostProcess, -1)]
    public class ReloadWeaponSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            InventoryComponent[] inventoryComponents = ((ComponentArray<InventoryComponent>)componentArrays[(int)ComponentType.Inventory]).Components;
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;


            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is ReloadWeaponPostProcessEvent weaponPostprocess)
                {
                    EquipmentComponent equipment = equipmentComponents[weaponPostprocess.WeaponHolderEntityId];

                    WeaponComponent weapon = weaponComponents[equipment.EquippedItemEntityIds[equipment.CurrentEquippedWeaponIndex]];

                    WeaponConfig weaponConfig = weapon.WeaponConfig;

                    //int ammoNeeded = Math.Min(weaponConfig.ReloadSize, weaponConfig.MagazineSize - weapon.AmmoCount.CurrentCount);

                    InventoryItemConfig ammoConfig = weaponConfig.AmmoConfig;

                    InventoryComponent inventoryComponent = inventoryComponents[weaponPostprocess.WeaponHolderEntityId];

                    weapon.AmmoCount.CurrentCount += inventoryComponent.Inventory.TryTakeFromStack(ammoConfig.Id, weaponConfig.ReloadSize);

                }
            }
        }
    }

}