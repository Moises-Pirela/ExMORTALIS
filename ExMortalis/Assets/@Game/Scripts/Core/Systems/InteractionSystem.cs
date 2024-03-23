using System.Collections.Generic;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess, -1)]
    public class InteractionSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            InteractableComponent[] interactableComponents = ((ComponentArray<InteractableComponent>)componentArrays[(int)ComponentType.Interactable]).Components;
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            InventoryComponent[] inventoryComponents = ((ComponentArray<InventoryComponent>)componentArrays[(int)ComponentType.Inventory]).Components;

            for (int i = 0; i < postProcessEvents.Count; i++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i];

                if (postProcessEvent is InteractPostprocessEvent interactPostprocess)
                {
                    if (World.Instance.EntityContainer.GetComponent<WeaponPickUpComponent>(interactPostprocess.TargetEntityId, ComponentType.WeaponPickUp, out WeaponPickUpComponent pickupComponent))
                    {
                        if (pickupComponent.WeaponConfig)
                        {
                            EquipWeaponPostProcessEvent equipWeaponPostProcessEvent = new EquipWeaponPostProcessEvent();
                            equipWeaponPostProcessEvent.ParentTransform = equipmentComponents[interactPostprocess.InteractorEntityId].WeaponSpawnPoint;
                            equipWeaponPostProcessEvent.WeaponConfigId = pickupComponent.WeaponConfig.Id;
                            equipWeaponPostProcessEvent.WielderEntityId = interactPostprocess.InteractorEntityId;
                            equipWeaponPostProcessEvent.WeaponSlotIndex = 0;

                            World.Instance.AddPostProcessEvent(equipWeaponPostProcessEvent);
                        }

                        continue;
                    }
                    else if (World.Instance.EntityContainer.GetComponent<AmmoItemPickupComponent>(interactPostprocess.TargetEntityId, ComponentType.AmmoItemPickup, out AmmoItemPickupComponent ammoPickup))
                    {
                        Debug.Log($"Ammo pickup {ammoPickup.ItemConfig.Name}");
                        InventoryItem inventoryItem = new InventoryItem()
                        {
                            ConfigId = ammoPickup.ItemConfig.Id
                        };
                        
                        inventoryComponents[interactPostprocess.InteractorEntityId].Inventory.TryAdd(inventoryItem);
                    }


                    interactableComponents[interactPostprocess.TargetEntityId].OnInteract.Invoke();
                }
            }
        }
    }

}