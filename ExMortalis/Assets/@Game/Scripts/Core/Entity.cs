using System;
using System.Collections;
using System.Security.Principal;
using NL.Core;
using NL.Core.Configs;
using NL.Core.Postprocess;
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
    public RenderConfig Render;
    [SerializeField] private bool IsStartingEntity = false;
    [HideInInspector] public int Id = -1;
    public EntityFlags Flags;

    private void Start()
    {
        if (IsStartingEntity)
        {

            CreateEntityPostprocessEvent createEntityPostprocess = new CreateEntityPostprocessEvent
            {
                EntityGO = this.gameObject,
                SpawnPosition = transform.position,
                SpawnRotation = transform.rotation.eulerAngles
            };

            World.Instance.AddPostProcessEvent(createEntityPostprocess);
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontSize = 16;
        Handles.Label(transform.position + Vector3.up / 2, $"ID : {Id}", style);
    }
#endif
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


