namespace Transendence.Core.Postprocess
{
    public enum WeaponUseType
    {
        Shoot,
        Reload,
        Inspect
    }
    public struct UseWeaponPostprocessEvent : IPostProcessEvent
    {
        public int WeaponIndex;
        public int WeaponHolderEntityId;
        public WeaponUseType WeaponUseType;
    }

    public struct CycleWeaponPostProcessEvent : IPostProcessEvent
    {
        public int CycleAmount;
        public int EntityCycleId;
    }
}


