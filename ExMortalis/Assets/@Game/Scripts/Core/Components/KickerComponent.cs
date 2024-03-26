using UnityEngine;

namespace NL.Core
{
    public class KickerComponent : MonoBehaviour, IComponent
    {
        public float KickCooldownTimeSecs;
        public Transform KickTransform;
        public float KickRadius;
        public float KickRange;
        public LayerMask KickMask;
        [HideInInspector] public float NextKickTime;
        [HideInInspector] public Vector3 KickDirection;
        public float KickForce;

        [HideInInspector] public int KickedEntityId = -1;
        [HideInInspector] public bool HasKicked;

        public ComponentType GetComponentType()
        {
            return ComponentType.Kicker;
        }
    }
}
