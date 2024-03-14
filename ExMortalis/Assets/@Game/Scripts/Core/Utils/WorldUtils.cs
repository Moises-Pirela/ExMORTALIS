using UnityEngine;

public static class WorldUtils
{
    public static Entity FindEntityInParent(Transform startTransform)
{
    Entity entity = null;
    Transform currentTransform = startTransform;

    while (currentTransform != null)
    {
        if (currentTransform.TryGetComponent<Entity>(out entity))
        {
            // Found the Entity component, exit the loop
            break;
        }
        // Move up to the parent
        currentTransform = currentTransform.parent;
    }

    return entity;
}
}