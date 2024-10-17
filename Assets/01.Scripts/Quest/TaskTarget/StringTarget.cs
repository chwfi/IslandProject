using UnityEngine;

[CreateAssetMenu(menuName = "SO/Quest/TaskTarget/String", fileName = "TaskTarget_")]
public class StringTarget : TaskTarget
{
    [SerializeField] private string _target;

    public override object Target => _target;

    public override bool IsTargetEqual(object target)
    {
        string StringTarget = target as string;
        if (_target == StringTarget)
            return true;
        else
            return false;
    }
}