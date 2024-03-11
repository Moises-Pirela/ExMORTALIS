using System.Collections.Generic;
using Transendence.Core.Postprocess;
using Transendence.Utilities;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class CycleEquipmentSystem : BaseSystem
    {
        public override void Update(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;

            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is CycleWeaponPostProcessEvent cycleWeaponPostprocess)
                {
                    int newWeaponIndex = equipmentComponents[cycleWeaponPostprocess.EntityCycleId].CurrentEquippedWeaponIndex + cycleWeaponPostprocess.CycleAmount;

                    if (newWeaponIndex >= equipmentComponents[cycleWeaponPostprocess.EntityCycleId].EquippedWeaponEntityIds.Length)
                    {
                        newWeaponIndex = 0;

                    }
                    else if (newWeaponIndex < 0)
                    {
                        newWeaponIndex = equipmentComponents[cycleWeaponPostprocess.EntityCycleId].EquippedWeaponEntityIds.Length - 1;
                    }

                    int equippedEntityId = equipmentComponents[cycleWeaponPostprocess.EntityCycleId].EquippedWeaponEntityIds[equipmentComponents[cycleWeaponPostprocess.EntityCycleId].CurrentEquippedWeaponIndex];
                    int toEquipEntityId = equipmentComponents[cycleWeaponPostprocess.EntityCycleId].EquippedWeaponEntityIds[newWeaponIndex];

                    weaponComponents[equippedEntityId].gameObject.SetActive(false);
                    weaponComponents[toEquipEntityId].gameObject.SetActive(true);

                    equipmentComponents[cycleWeaponPostprocess.EntityCycleId].PrevEquippedWeaponIndex = equipmentComponents[cycleWeaponPostprocess.EntityCycleId].CurrentEquippedWeaponIndex;
                    equipmentComponents[cycleWeaponPostprocess.EntityCycleId].CurrentEquippedWeaponIndex = newWeaponIndex;

                }
            }
        }
    }

}