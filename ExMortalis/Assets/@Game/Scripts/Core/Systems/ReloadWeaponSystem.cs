using System;
using System.Collections.Generic;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using UnityEngine.Android;
using UnityEngine.UIElements;

namespace Transendence.Core.Systems
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

                    if (equipment.CurrentEquippedWeaponIndex == -1) continue;

                    WeaponComponent weapon = weaponComponents[equipment.EquippedItemEntityIds[equipment.CurrentEquippedWeaponIndex]];
                    WeaponConfig weaponConfig = weapon.WeaponConfig;

                    if (weapon.AmmoCount.CurrentCount >= weaponConfig.MagazineSize) continue;

                    //int ammoNeeded = Math.Min(weaponConfig.ReloadSize, weaponConfig.MagazineSize - weapon.AmmoCount.CurrentCount);

                    InventoryItemConfig ammoConfig = weaponConfig.AmmoConfig;

                    InventoryComponent inventoryComponent = inventoryComponents[weaponPostprocess.WeaponHolderEntityId];

                    int stacks = inventoryComponent.Inventory.GetStacksOfItem(ammoConfig.Id);

                    if (stacks == 0) continue;

                    weapon.AmmoCount.CurrentCount += inventoryComponent.Inventory.TryTakeFromStack(ammoConfig.Id, weaponConfig.ReloadSize);
                }
            }
        }
    }

}