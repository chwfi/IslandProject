using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    public abstract object Target { get; }

    public abstract bool IsTargetEqual(object target);
}