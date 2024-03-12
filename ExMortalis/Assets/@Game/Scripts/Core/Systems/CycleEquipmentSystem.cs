using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class CycleEquipmentSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;

            foreach (var postProcessEvent in postProcessEvents)
            {
                if (postProcessEvent is not CycleWeaponPostProcessEvent cycleWeaponPostprocess)
                    continue;

                int entityCycleId = cycleWeaponPostprocess.EntityCycleId;
                var equipment = equipmentComponents[entityCycleId];
                int currentWeaponIndex = equipment.CurrentEquippedWeaponIndex;

                if (currentWeaponIndex == -1) continue;

                int newWeaponIndex = currentWeaponIndex + cycleWeaponPostprocess.CycleAmount;

                int equippedEntityId = equipment.EquippedItemEntityIds[currentWeaponIndex];

                if (equippedEntityId == -1) continue;

                int toEquipEntityId;

                if (newWeaponIndex < 0)
                    newWeaponIndex = equipment.EquippedItemEntityIds.Length - 1;
                else if (newWeaponIndex >= equipment.EquippedItemEntityIds.Length)
                    newWeaponIndex = 0;

                toEquipEntityId = equipment.EquippedItemEntityIds[newWeaponIndex];

                if (toEquipEntityId == -1)
                {
                    int startIndex = cycleWeaponPostprocess.CycleAmount > 0 ? 0 : equipment.EquippedItemEntityIds.Length - 1;
                    int direction = cycleWeaponPostprocess.CycleAmount > 0 ? 1 : -1;

                    for (int i = startIndex; i >= 0 && i < equipment.EquippedItemEntityIds.Length; i += direction)
                    {
                        if (equipment.EquippedItemEntityIds[i] != -1)
                        {
                            newWeaponIndex = i;
                            toEquipEntityId = equipment.EquippedItemEntityIds[newWeaponIndex];
                            break;
                        }
                    }
                }

                weaponComponents[equippedEntityId].gameObject.SetActive(false);
                weaponComponents[toEquipEntityId].gameObject.SetActive(true);

                equipment.PrevEquippedWeaponIndex = currentWeaponIndex;
                equipment.CurrentEquippedWeaponIndex = newWeaponIndex;
            }
        }
    }

}