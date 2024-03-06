using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transendence.Core.Configs
{
    public enum WeaponType { Ranged, Melee, Throwable }
    public enum AmmoType { Kinetic, Neurobiotic, Energy }

    [CreateAssetMenu(menuName = "Tools/Configs/Weapons", fileName = "Weapon")]
    public class WeaponConfig : EquipmentConfig
    {
        public WeaponType WeaponType;
        public AmmoType AmmoType;
        public float MaxRange;
        public float MaxRounds;
        public float MagazineSize;
        public float RecoilStrength;
        public float BloomSize;
        public Texture ReticleTexture;
    }
}