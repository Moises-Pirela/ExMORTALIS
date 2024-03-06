using System;
using System.Security.Principal;
using Transendence.Core;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum EEntityFlags
{
    Active,
    Dead,
    Inactive
}

public class Entity : MonoBehaviour
{
    [SerializeField] private bool IsStartingEntity = false;
    [HideInInspector] public int Id = -1;
    public EntityFlags Flags;

    private void Start()
    {
        if (IsStartingEntity)
        {
            World.Instance.EntityContainer.CreateEntity(this.gameObject);            
        }
    }

    public bool IsDead()
    {
        return Flags.HasFlag((int)EEntityFlags.Dead);
    }

    public bool IsActive()
    {
        return Flags.HasFlag((int)EEntityFlags.Active);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsActive()) return;

        if (!collision.gameObject.TryGetComponent<Entity>(out Entity collisionEntity))
        {
            collisionEntity = this;
        }

        if (World.Instance.EntityContainer.GetComponent<CollisionComponent>(Id, ComponentType.Collision, out CollisionComponent collisionComponent))
        {
            CollisionInfo collisionInfo = new CollisionInfo()
            {
                CollidedEntityId = collisionEntity.Id,
                ContactPoint = collision.contacts[0].point,
                HasCollided = true
            };

            collisionComponent.Collisions.TryAdd(collisionEntity.Id, collisionInfo);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsActive()) return;

        if (!collision.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            entity = this;
        }

        if (World.Instance.EntityContainer.GetComponent<CollisionComponent>(Id, ComponentType.Collision, out CollisionComponent collisionComponent))
        {
            collisionComponent.Collisions.Remove(entity.Id);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.Label(transform.position + Vector3.up / 2, $"ID : {Id}", new GUIStyle() { fontSize = 16 });
    }
}

public struct EntityFlags
{
    public int Value;

    public EntityFlags(params int[] flags)
    {
        Value = 0;

        for (int i = 0; i < flags.Length; i++)
        {
            SetFlag(flags[i]);
        }
    }

    public bool HasFlag(int flag)
    {
        return (Value & (1 << flag)) != 0;
    }

    public void SetFlag(int flag)
    {
        Value |= (1 << flag);
    }

    public void RemoveFlag(int flag)
    {
        Value &= ~(1 << flag);
    }
}


