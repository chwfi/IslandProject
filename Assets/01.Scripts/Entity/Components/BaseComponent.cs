using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    protected Entity _owner;

    public void SetOwner(Entity entity)
    {
        _owner = entity;
    }
}