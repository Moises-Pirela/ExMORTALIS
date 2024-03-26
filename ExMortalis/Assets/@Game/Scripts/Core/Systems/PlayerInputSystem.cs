using JetBrains.Annotations;
using NL.Core.Configs;
using NL.Core.Postprocess;
using NL.Utilities;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace NL.Core.Systems
{
    [System(SystemAttributeType.PostProcess, -1000)]
    public class PlayerInputSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is PlayerInputPostProcess inputPostprocess)
                {
                    switch (inputPostprocess.InputType)
                    {
                        case PlayerInputPostProcess.PlayerInputType.Reload:

                            World.Instance.EntityContainer.GetComponent<EquipmentComponent>(World.PLAYER_ENTITY_ID, ComponentType.Equipment, out EquipmentComponent equipment);

                            if (equipment.CurrentEquippedWeaponIndex == -1) return;

                            World.Instance.EntityContainer.GetComponent<WeaponComponent>(equipment.EquippedItemEntityIds[equipment.CurrentEquippedWeaponIndex], ComponentType.Weapon, out WeaponComponent weapon);

                            World.Instance.EntityContainer.GetComponent<InventoryComponent>(World.PLAYER_ENTITY_ID, ComponentType.Inventory, out InventoryComponent inventory);

                            if (inventory.Inventory.GetStacksOfItem(weapon.WeaponConfig.AmmoConfig.Id) == 0) return;

                            if (weapon.AmmoCount.CurrentCount >= weapon.WeaponConfig.MagazineSize) return;

                            if (weapon.NextReloadTimeSec > Time.time) return;

                            weapon.SetAnimation("Reload", "Reload");

                            weapon.NextReloadTimeSec = Time.time + weapon.WeaponConfig.ReloadTimeSec;

                            break;
                        case PlayerInputPostProcess.PlayerInputType.Interact:

                            InteractPostprocessEvent interactPostprocessEvent = new InteractPostprocessEvent();

                            interactPostprocessEvent.InteractorEntityId = World.PLAYER_ENTITY_ID;
                            interactPostprocessEvent.TargetEntityId = inputPostprocess.InteractionEntityId;
                            interactPostprocessEvent.Type = InteractPostprocessEvent.InteractionType.Use;

                            World.Instance.AddPostProcessEvent(interactPostprocessEvent);

                            break;
                        case PlayerInputPostProcess.PlayerInputType.Fire:

                            World.Instance.EntityContainer.GetComponent<ThrowerComponent>(World.PLAYER_ENTITY_ID, ComponentType.Thrower, out ThrowerComponent thrower);

                            if (thrower.PickedUpEntityId != -1)
                            {
                                int pickedUpEntity = thrower.PickedUpEntityId;

                                World.Instance.EntityContainer.GetComponent<ThrowableComponent>(thrower.PickedUpEntityId, ComponentType.Throwable, out ThrowableComponent throwable);

                                thrower.ThrowDirection = thrower.PickupTransform.forward;
                                thrower.ThrowDistance = 1000; //TODO: Should be replaced with current character strength skill;

                                return;
                            }

                            UseWeaponPostprocessEvent use = new UseWeaponPostprocessEvent();

                            use.WeaponHolderEntityId = World.PLAYER_ENTITY_ID;
                            use.WeaponUseType = WeaponUseType.Shoot;

                            World.Instance.AddPostProcessEvent(use);

                            break;
                        case PlayerInputPostProcess.PlayerInputType.SecondaryFire:

                            World.Instance.EntityContainer.GetComponent<KickerComponent>(World.PLAYER_ENTITY_ID, ComponentType.Kicker, out KickerComponent kicker);

                            kicker.HasKicked = true;

                            break;
                    }
                }
            }
        }
    }

    public struct PlayerInputPostProcess : IPostProcessEvent
    {
        public enum PlayerInputType { Reload, Interact, Fire, SecondaryFire }

        public PlayerInputType InputType;

        public int InteractionEntityId;
    }
}