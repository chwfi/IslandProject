using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthState
{
    Seed,
    Middle,
    WaitingForCompletion
}

public class ObjectField : PlaceableObject
{
    public GrowthState GrowthState { get; private set; }

    private float _timer;

    public override void OnPlace()
    {
        base.OnPlace();

        GrowthState = GrowthState.Seed;
        FieldStateManager.Instance.FieldList.Add(this);
    }

    public void SetTimer(float endTime)
    {
        if (GrowthState == GrowthState.WaitingForCompletion)
            return;

        _timer += Time.deltaTime;

        if (_timer >= endTime)
        {
            switch (GrowthState)
            {
                case GrowthState.Seed:
                    GrowthState = GrowthState.Middle;
                    _timer = 0;
                    break;
                case GrowthState.Middle:
                    GrowthState = GrowthState.WaitingForCompletion;
                    _timer = 0;
                    break;
                case GrowthState.WaitingForCompletion:
                    break;
            }
        }
    }
}