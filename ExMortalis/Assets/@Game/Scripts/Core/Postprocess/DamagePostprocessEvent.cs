namespace Transendence.Core.Postprocess
{
    public struct DamagePostprocessEvent : IPostProcessEvent
    {
        public int TargetEntityId;
        public int DamageDealerEntityId;
        public float Damage;
    }
}


