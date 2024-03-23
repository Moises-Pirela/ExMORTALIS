using JetBrains.Annotations;
using NL.Core.Configs;
using NL.Core.Postprocess;
using NL.Utilities;
using System.Collections.Generic;
using UnityEngine;

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



                            break;
                        case PlayerInputPostProcess.PlayerInputType.Fire:



                            break;
                    }
                }
            }
        }
    }

    public struct PlayerInputPostProcess : IPostProcessEvent
    {
        public enum PlayerInputType { Reload, Interact, Fire }

        public PlayerInputType InputType;
    }
}