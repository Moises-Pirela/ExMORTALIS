using UnityEngine;

namespace Transendence.Core.Postprocess
{
    public struct DamagePostprocessEvent : IPostProcessEvent
    {
        public int TargetEntityId;
        public int DamageDealerEntityId;
        public float Damage;
        public Vector3 KnockbackForce;
    }

    public struct CreateEntityPostprocessEvent : IPostProcessEvent
    {
        public GameObject EntityGO;
        public Vector3 SpawnPosition;
        public Vector3 SpawnRotation;
    }

    public struct KillEntityPostprocessEvent : IPostProcessEvent
    {
        public int EntityId;
        public bool DestroyGO;
        public float DestroyTimer;
    }
}


