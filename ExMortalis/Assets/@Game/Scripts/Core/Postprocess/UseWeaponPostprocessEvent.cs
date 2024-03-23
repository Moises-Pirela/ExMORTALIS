using UnityEngine;

namespace Transendence.Core.Postprocess
{
    public enum WeaponUseType
    {
        Shoot,
        Reload,
        Inspect
    }

    public struct ReloadWeaponPostProcessEvent : IPostProcessEvent
    {
        public int WeaponHolderEntityId;
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

    public struct EquipWeaponPostProcessEvent : IPostProcessEvent
    {
        public int WielderEntityId;
        public int WeaponConfigId;
        public int WeaponSlotIndex;
        public Transform ParentTransform;
    }
}


