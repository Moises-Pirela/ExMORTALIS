using System.Collections.Generic;
using NL.Core.Configs;
using NL.Core.Postprocess;
using NL.Utilities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace NL.Core.Systems
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

                        if (World.Instance.EntityContainer.HasComponent<DestroyOnPickupComponent>(interactPostprocess.TargetEntityId, ComponentType.DestroyOnPickup))
                        {
                            KillEntityPostprocessEvent killEntityPostprocess = new KillEntityPostprocessEvent();

                            killEntityPostprocess.EntityId = interactPostprocess.TargetEntityId;
                            killEntityPostprocess.DestroyGO = true;

                            World.Instance.AddPostProcessEvent(killEntityPostprocess);
                        }

                        continue;
                    }
                    else if (World.Instance.EntityContainer.GetComponent<AmmoItemPickupComponent>(interactPostprocess.TargetEntityId, ComponentType.AmmoItemPickup, out AmmoItemPickupComponent ammoPickup))
                    {
                        InventoryItem inventoryItem = new InventoryItem()
                        {
                            ConfigId = ammoPickup.ItemConfig.Id
                        };

                        inventoryComponents[interactPostprocess.InteractorEntityId].Inventory.TryAdd(inventoryItem);

                        if (World.Instance.EntityContainer.HasComponent<DestroyOnPickupComponent>(interactPostprocess.TargetEntityId, ComponentType.DestroyOnPickup))
                        {
                            KillEntityPostprocessEvent killEntityPostprocess = new KillEntityPostprocessEvent();

                            killEntityPostprocess.EntityId = interactPostprocess.TargetEntityId;
                            killEntityPostprocess.DestroyGO = true;

                            World.Instance.AddPostProcessEvent(killEntityPostprocess);
                        }

                        continue;
                    }
                    else if (World.Instance.EntityContainer.GetComponent<ThrowableComponent>(interactPostprocess.TargetEntityId, ComponentType.Throwable, out ThrowableComponent throwableComponent))
                    {
                        if (World.Instance.EntityContainer.GetComponent<ThrowerComponent>(interactPostprocess.InteractorEntityId, ComponentType.Thrower, out ThrowerComponent throwerComponent))
                        {
                            if (throwerComponent.PickedUpEntityId == -1)
                            {
                                throwerComponent.PickedUpEntityId = interactPostprocess.TargetEntityId;

                                throwableComponent.transform.position = throwerComponent.PickupTransform.position;
                                throwableComponent.transform.parent = throwerComponent.PickupTransform;
                                throwableComponent.Rigidbody.useGravity = false;
                            }
                            else
                            {
                                Physics.IgnoreCollision(throwerComponent.GetComponent<Collider>(), throwableComponent.GetComponent<Collider>(), false);
                                throwableComponent.Rigidbody.useGravity = true;
                                throwableComponent.transform.parent = null;
                                throwerComponent.PickedUpEntityId = -1;
                            }
                        }
                    }


                    interactableComponents[interactPostprocess.TargetEntityId].OnInteract.Invoke();
                }
            }
        }
    }

}