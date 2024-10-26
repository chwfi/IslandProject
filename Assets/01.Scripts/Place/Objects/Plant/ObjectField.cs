using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public enum GrowthState
{
    Seed,
    Middle,
    Complete
}

public class ObjectField : PlaceableObject
{
    public GrowthState GrowthState { get; private set; }

    private PlantGroup _plantGroup;
    private float _timer;

    public override void OnPlace()
    {
        base.OnPlace();

        if (placed)
        {
            _plantGroup = transform.Find("PlantGroup").GetComponent<PlantGroup>();
            FieldStateManager.Instance.FieldList.Add(this);
            InitPlants();
        }
    }

    private void InitPlants()
    {
        GrowthState = GrowthState.Seed;
        _plantGroup.SetPlants(GrowthState, _objectData.objectName);
    }

    public void SetTimer(float endTime)
    {
        if (GrowthState == GrowthState.Complete)
            return;

        _timer += Time.deltaTime;

        if (_timer >= endTime)
        {
            CheckState();
        }
    }

    private void CheckState()
    {
        if (GrowthState == GrowthState.Seed)
        {
            GrowthState = GrowthState.Middle;
            _plantGroup.SetPlants(GrowthState, _objectData.objectName);
            _timer = 0;
            return;
        }

        if (GrowthState == GrowthState.Middle)
        {
            GrowthState = GrowthState.Complete;
            _plantGroup.SetPlants(GrowthState, _objectData.objectName);
            _timer = 0;

            var ui = PoolManager.Instance.Take("HarvestButtonUI", transform) as HarvestButtonUI;
            ui.SetObject(this);
            return;
        }
    }

    public void OnHarvest()
    {
        MaterialManager.Instance.AddMaterialCount(_objectData.material, _plantGroup.PlantPositions.Length);
        InitPlants();
    }
}