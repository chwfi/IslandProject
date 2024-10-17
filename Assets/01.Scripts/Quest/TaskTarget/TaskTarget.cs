using UnityEngine;

public abstract class TaskTarget : ScriptableObject, IQuestable
{
    public abstract object Target { get; }

    public abstract bool IsTargetEqual(object target);
}