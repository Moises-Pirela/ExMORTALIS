using System.Collections.Generic;
using JetBrains.Annotations;
using NL.Core.Configs;
using NL.Core.Postprocess;
using NL.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace NL.Core.Systems
{

    [System(SystemAttributeType.PostProcess)]
    public class EquipWeaponSystem : BaseSystem
    {
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;
            ChildComponent[] childComponents = ((ComponentArray<ChildComponent>)componentArrays[(int)ComponentType.Child]).Components;

            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is EquipWeaponPostProcessEvent weaponPostprocess)
                {
                    int wielderEntityId = weaponPostprocess.WielderEntityId;
                    WeaponConfig weaponConfig = World.Instance.WorldConfig.InventoryItemConfigs[weaponPostprocess.WeaponConfigId] as WeaponConfig;

                    var weaponInstance = GameObject.Instantiate(weaponConfig.WeaponEntityPrefab) as GameObject;
                    int weaponEntityId = World.Instance.EntityContainer.CreateEntity(weaponInstance);

                    childComponents[weaponEntityId].ParentEntityId = weaponPostprocess.WielderEntityId;
                    entities[weaponEntityId].transform.parent = weaponPostprocess.ParentTransform;
                    entities[weaponEntityId].transform.localPosition = Vector3.zero + childComponents[weaponEntityId].SpawnOffset;
                    entities[weaponEntityId].transform.localRotation = Quaternion.Euler(childComponents[weaponEntityId].SpawnRotation);

                    //look for available slot for weapo
                    int availableSlot = -1;

                    for (int j = 0; j < equipmentComponents[wielderEntityId].EquippedItemEntityIds.Length; j++)
                    {
                        if (equipmentComponents[wielderEntityId].EquippedItemEntityIds[j] == -1)
                        {
                            availableSlot = j;
                            break;
                        }
                    }

                    if (availableSlot == -1) continue;

                    int currentWeaponEquipped = equipmentComponents[weaponPostprocess.WielderEntityId].CurrentEquippedWeaponIndex;

                    if (currentWeaponEquipped != -1)
                    {
                        int equippedEntityId = equipmentComponents[weaponPostprocess.WielderEntityId].EquippedItemEntityIds[currentWeaponEquipped];

                        weaponComponents[equippedEntityId].gameObject.SetActive(false);
                    }

                    equipmentComponents[wielderEntityId].EquippedItemEntityIds[availableSlot] = weaponEntityId;
                    equipmentComponents[weaponPostprocess.WielderEntityId].UpdateWeaponEquipped(availableSlot);
                }
            }
        }
    }

}