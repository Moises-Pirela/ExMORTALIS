using System;
using System.Collections;
using System.Collections.Generic;
using Transendence.Core.Configs;
using UnityEngine;

namespace Transendence.Core.Configs
{
    public enum SurfaceTags
    {
        Flesh, 
        Concrete,
        Water,
        Wood,
        Metal,
    }
    [CreateAssetMenu(menuName = "Configs/EffectsConfig")]
    public class EffectsLookupConfig : BaseScriptableConfig
    {
        public BulletImpactEffects[] BulletImpactEffects;
        public FootstepSFX[] FootstepSFX;
    }

    [Serializable]
    public class BulletImpactEffects
    {
        public SurfaceTags Tag;
        public GameObject ImpactEffectPrefab;
        public GameObject ImpactDecalPrefab;
    }

    [Serializable]
    public class FootstepSFX
    {
        public SurfaceTags Tag;
        public AudioClip FootstepSound;
    }
}
