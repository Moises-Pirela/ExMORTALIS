using System.Collections.Generic;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Transendence.Core.Systems
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
                    WeaponConfig weaponConfig = World.Instance.WorldConfig.WeaponConfigs[weaponPostprocess.WeaponConfigId];

                    var weaponInstance = GameObject.Instantiate(weaponConfig.WeaponEntityPrefab) as GameObject;
                    int weaponEntityId = World.Instance.EntityContainer.CreateEntity(weaponInstance);

                    childComponents[weaponEntityId].ParentEntityId = weaponPostprocess.WielderEntityId;
                    entities[weaponEntityId].transform.parent = weaponPostprocess.ParentTransform;
                    entities[weaponEntityId].transform.localPosition = Vector3.zero + childComponents[weaponEntityId].SpawnOffset;
                    entities[weaponEntityId].transform.localRotation = Quaternion.Euler(childComponents[weaponEntityId].SpawnRotation);

                    equipmentComponents[wielderEntityId].EquippedWeaponEntityIds[weaponPostprocess.WeaponSlotIndex] = weaponEntityId;
                }
            }
        }
    }

}