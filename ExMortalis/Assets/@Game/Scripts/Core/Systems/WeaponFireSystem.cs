using System.Collections.Generic;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess)]
    public class WeaponFireSystem : BaseSystem
    {
        private float NextFireTime;
        public override void Update(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;

            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is UseWeaponPostprocessEvent weaponPostprocess)
                {
                    int weaponIndex = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].CurrentEquippedWeaponIndex;
                    Weapon equippedWeapon = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].EquippedWeapons[weaponIndex];
                    WeaponConfig weaponConfig = World.Instance.WorldConfig.WeaponConfigs[equippedWeapon.ConfigId];
                    Transform weaponTransform = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].WeaponSpawnPoint;

                    if (weaponPostprocess.WeaponUseType == WeaponUseType.Shoot)
                    {
                        if (NextFireTime > Time.time) continue;

                        // if (equippedWeapon.AmmoCount.CurrentCount <= 0)
                        // {
                        //     equippedWeapon.PlayOneShot(weaponConfig.EmptyFireClip);
                        //     continue;
                        // } 

                        NextFireTime = weaponConfig.FireRate + Time.time;

                        for (int i = 0; i < Mathf.Max(1, weaponConfig.BulletsPerShot); i++)
                        {
                            Vector3 bloom = weaponTransform.position + weaponTransform.forward * weaponConfig.MaxRange;

                            if (weaponConfig.BloomSize > 0)
                            {
                                bloom += UnityEngine.Random.Range(-weaponConfig.BloomSize, weaponConfig.BloomSize) * weaponTransform.up;
                                bloom += UnityEngine.Random.Range(-weaponConfig.BloomSize, weaponConfig.BloomSize) * weaponTransform.right;
                                bloom -= weaponTransform.position;
                                bloom.Normalize();
                            }

                            RaycastHit hit = new RaycastHit();
                            if (Physics.Raycast(weaponTransform.position, bloom, out hit, weaponConfig.MaxRange, equipmentComponents[weaponPostprocess.WeaponHolderEntityId].ShootLayer))
                            {
                                hit.collider.gameObject.TryGetComponent(out Entity hitEntity);

                                if (hitEntity != null && World.Instance.EntityContainer.HasComponent<HealthComponent>(hitEntity.Id, ComponentType.Health))
                                {
                                    DamagePostprocessEvent damagePostprocessEvent = new DamagePostprocessEvent()
                                    {
                                        DamageDealerEntityId = weaponPostprocess.WeaponHolderEntityId,
                                        Damage = weaponConfig.Damage,
                                        TargetEntityId = hitEntity.Id
                                    };

                                    World.Instance.AddPostProcessEvent(damagePostprocessEvent);
                                }
                            }
                        }

                        equippedWeapon.AmmoCount.CurrentCount--;
                        int randomClip = UnityEngine.Random.Range(0, weaponConfig.FireClips.Length);
                        equippedWeapon.PlayOneShot(weaponConfig.FireClips[randomClip]);
                    }
                }
            }
        }
    }

}