using UnityEngine;

namespace NL.Core
{
    public class PlayerTagComponent : MonoBehaviour, IComponent
    {
        public Camera PlayerCamera;

        public ComponentType GetComponentType()
        {
            return ComponentType.PlayerTag;
        }

        public Vector3 GetForward()
        {
            return PlayerCamera.transform.position + PlayerCamera.transform.forward;
        }
    }
}
