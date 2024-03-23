using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.Core.Configs
{
    public enum WeaponType { RANGED, MELEE, THROWABLE }
    public enum AmmoWeaponType { SHOTGUN, PISTOL, MAGNUM }

    [CreateAssetMenu(menuName = "Tools/Configs/Weapons", fileName = "Weapon")]
    public class WeaponConfig : EquipmentConfig
    {
        [Header("Weapon")]
        public GameObject WeaponEntityPrefab;
        public WeaponType WeaponType;
        public DamageType AmmoType;
        public InventoryItemConfig AmmoConfig;
        public float Damage;
        public float FireRate;
        public float ReloadTimeSec;
        public int BulletsPerShot;
        public float MaxRange;
        public int MagazineSize;
        public int ReloadSize;
        public float RecoilStrength;
        public float BloomSize;
        [Range(1, 100)] public float KnockbackForce;
        public Texture ReticleTexture;
        public AudioClip[] FireClips;
        public AudioClip EmptyFireClip;
    }
}
