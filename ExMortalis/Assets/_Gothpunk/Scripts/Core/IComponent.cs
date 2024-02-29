﻿namespace Transendence.Core
{
    public enum ComponentType
    {
        Movement = 0,
        Health,
        Ability,
        Damage,
        Child,
        Projectile,
        BuffDebuff,
        Throwable,
        PlayerTag,
        DestroyOnHit,
        SpawnOnHit,
        DamageOnHit,
        Collision,
        Attributes,
        BuffOnHit,
        Interactable,
        Flammable,
        Max,
    }

    public interface IComponent
    {
        public ComponentType GetComponentType();
    }
}