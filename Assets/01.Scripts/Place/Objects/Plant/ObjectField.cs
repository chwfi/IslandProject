using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Util;

public class ObjectField : PlaceableObject
{
    [SerializeField] private InGameMaterial _harvestMaterial;
    [SerializeField] private GameObject[] _plantGroups;

    public Vector3[] GetPlantPositions()
    {
        return transform.Find("PlantGroup").GetComponent<PlantGroup>().PlantPositions;
    }

    public override void OnInactive()
    {
        foreach (var obj in _plantGroups)
        {
            obj.SetActive(false);
        }

        _plantGroups[0].SetActive(true);
    }

    public override void OnActive()
    {
        foreach (var obj in _plantGroups)
        {
            obj.SetActive(false);
        }

        _plantGroups[1].SetActive(true);
    }

    public override void OnWaitForCompletion()
    {
        foreach (var obj in _plantGroups)
        {
            obj.SetActive(false);
        }

        _plantGroups[2].SetActive(true);

        ShowHarvestUI(_harvestMaterial);
    }

    public override void OnHarvest()
    {
        MaterialManager.Instance.AddMaterialCount(_harvestMaterial, 1);

        ObjectState = PlaceableObjectState.Inactive;
        OnInactive();
        
        _timer = 0;
    }
}