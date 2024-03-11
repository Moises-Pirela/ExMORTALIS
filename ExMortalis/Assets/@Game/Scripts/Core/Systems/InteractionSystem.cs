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
        public override void Update(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            InteractableComponent[] interactableComponents = ((ComponentArray<InteractableComponent>)componentArrays[(int)ComponentType.Interactable]).Components;
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            InventoryComponent[] inventoryComponents = ((ComponentArray<InventoryComponent>)componentArrays[(int)ComponentType.Inventory]).Components;

            for (int i = 0; i < postProcessEvents.Count; i++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i];

                if (postProcessEvent is InteractPostprocessEvent interactPostprocess)
                {
                    if (World.Instance.EntityContainer.GetComponent<PickUpOnInteractComponent>(interactPostprocess.TargetEntityId, ComponentType.PickUpOnInteract, out PickUpOnInteractComponent pickupComponent))
                    {
                        if (pickupComponent.InventoryItemConfig is WeaponConfig)
                        {
                            EquipWeaponPostProcessEvent equipWeaponPostProcessEvent = new EquipWeaponPostProcessEvent();
                            equipWeaponPostProcessEvent.ParentTransform = equipmentComponents[interactPostprocess.InteractorEntityId].WeaponSpawnPoint;
                            equipWeaponPostProcessEvent.WeaponConfigId = pickupComponent.InventoryItemConfig.Id;
                            equipWeaponPostProcessEvent.WielderEntityId = interactPostprocess.InteractorEntityId;
                            equipWeaponPostProcessEvent.WeaponSlotIndex = 0;

                            World.Instance.AddPostProcessEvent(equipWeaponPostProcessEvent);
                        }
                        else if (pickupComponent.InventoryItemConfig is EquipmentConfig)
                        {
                            //Equipment
                        }
                        else 
                        {
                            //InventoryItem
                        }

                        continue;
                    }


                    interactableComponents[interactPostprocess.TargetEntityId].OnInteract.Invoke();
                }
            }
        }
    }

}