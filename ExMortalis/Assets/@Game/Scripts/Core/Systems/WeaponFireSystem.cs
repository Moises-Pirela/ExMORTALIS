using System.Collections.Generic;
using PampelGames.GoreSimulator;
using Transendence.Core.Configs;
using Transendence.Core.Postprocess;
using Transendence.Utilities;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Transendence.Core.Systems
{
    [System(SystemAttributeType.PostProcess, -1)]
    public class WeaponFireSystem : BaseSystem
    {
        private float NextFireTime;
        public override void SystemUpdate(Entity[] entities, ComponentArray[] componentArrays, List<IPostProcessEvent> postProcessEvents)
        {
            EquipmentComponent[] equipmentComponents = ((ComponentArray<EquipmentComponent>)componentArrays[(int)ComponentType.Equipment]).Components;
            WeaponComponent[] weaponComponents = ((ComponentArray<WeaponComponent>)componentArrays[(int)ComponentType.Weapon]).Components;
            GoreComponent[] goreComponents = ((ComponentArray<GoreComponent>)componentArrays[(int)ComponentType.Gore]).Components;

            for (int i1 = 0; i1 < postProcessEvents.Count; i1++)
            {
                IPostProcessEvent postProcessEvent = postProcessEvents[i1];

                if (postProcessEvent is UseWeaponPostprocessEvent weaponPostprocess)
                {
                    int weaponIndex = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].CurrentEquippedWeaponIndex;
                    int weaponEntityId = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].EquippedItemEntityIds[weaponIndex];

                    WeaponComponent equippedWeapon = weaponComponents[weaponEntityId];

                    if (equippedWeapon == null) continue;

                    WeaponConfig weaponConfig = equippedWeapon.WeaponConfig;

                    Transform weaponShootTransform = equipmentComponents[weaponPostprocess.WeaponHolderEntityId].WeaponShootPoint;

                    if (weaponPostprocess.WeaponUseType == WeaponUseType.Shoot)
                    {
                        if (NextFireTime > Time.time) continue;

                        // if (equippedWeapon.AmmoCount.CurrentCount <= 0)
                        // {
                        //     equippedWeapon.PlayOneShot(weaponConfig.EmptyFireClip);
                        //     continue;
                        // } 

                        NextFireTime = weaponConfig.FireRate + Time.time;

                        float totalDamage = 0;
                        Entity hitEntity = null;
                        Vector3 knockbackDirection = Vector3.zero;

                        for (int i = 0; i < Mathf.Max(1, weaponConfig.BulletsPerShot); i++)
                        {
                            Vector3 bloom = weaponShootTransform.position + weaponShootTransform.forward * weaponConfig.MaxRange;

                            if (weaponConfig.BloomSize > 0)
                            {
                                bloom += UnityEngine.Random.Range(-weaponConfig.BloomSize, weaponConfig.BloomSize) * weaponShootTransform.up;
                                bloom += UnityEngine.Random.Range(-weaponConfig.BloomSize, weaponConfig.BloomSize) * weaponShootTransform.right;
                                bloom -= weaponShootTransform.position;
                                bloom.Normalize();
                            }

                            RaycastHit hit = new RaycastHit();
                            float headShotMultiplier = 1;

                            if (Physics.Raycast(weaponShootTransform.position, bloom, out hit, weaponConfig.MaxRange, equipmentComponents[weaponPostprocess.WeaponHolderEntityId].ShootLayer))
                            {
                                for (int effectIndex = 0; effectIndex < World.Instance.WorldConfig.EffectsLookupConfig.BulletImpactEffects.Length; effectIndex++)
                                {
                                    SurfaceTags surfaceTag = World.Instance.WorldConfig.EffectsLookupConfig.BulletImpactEffects[effectIndex].Tag;

                                    if (hit.collider.CompareTag(surfaceTag.ToString()))
                                    {
                                        var effect = GameObject.Instantiate(World.Instance.WorldConfig.EffectsLookupConfig.BulletImpactEffects[effectIndex].ImpactEffectPrefab);

                                        effect.transform.position = hit.point;
                                        Quaternion rotation = Quaternion.LookRotation(hit.normal);
                                        effect.transform.rotation = rotation;
                                    }

                                    if (hit.collider.CompareTag("Head"))
                                    {
                                        headShotMultiplier = 2;
                                    }
                                }

                                if (hitEntity == null)
                                {
                                    hitEntity = WorldUtils.FindEntityInParent(hit.transform);
                                }

                                if (hit.collider.gameObject.TryGetComponent(out Rigidbody rigidbody))
                                {
                                    knockbackDirection = (rigidbody.transform.position - weaponShootTransform.position).normalized;

                                    rigidbody.AddForce(knockbackDirection * weaponConfig.KnockbackForce, ForceMode.Impulse);
                                }

                                if (hitEntity != null && World.Instance.EntityContainer.HasComponent<HealthComponent>(hitEntity.Id, ComponentType.Health))
                                {
                                    if (goreComponents[hitEntity.Id])
                                    {
                                        var effect = GameObject.Instantiate(goreComponents[hitEntity.Id].HitEffectPrefab);

                                        effect.transform.position = hit.point;
                                        Quaternion rotation = Quaternion.LookRotation(hit.normal);
                                        effect.transform.rotation = rotation;
                                    }

                                    totalDamage += weaponConfig.Damage * headShotMultiplier;
                                }
                            }
                        }

                        if (totalDamage > 0)
                        {
                            DamagePostprocessEvent damagePostprocessEvent = new DamagePostprocessEvent()
                            {
                                DamageDealerEntityId = weaponPostprocess.WeaponHolderEntityId,
                                Damage = totalDamage,
                                TargetEntityId = hitEntity.Id,
                                KnockbackForce = knockbackDirection * weaponConfig.KnockbackForce
                            };

                            World.Instance.AddPostProcessEvent(damagePostprocessEvent);
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