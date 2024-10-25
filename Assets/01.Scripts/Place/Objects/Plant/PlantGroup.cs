using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlantGroup : MonoBehaviour
{
    private readonly Vector3[] plantPositions = 
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 0, -1)
    };

    public Vector3[] PlantPositions => plantPositions;

    public void SetPlants(GrowthState state, string objName)
    {
        PoolableMono[] childObjs = GetComponentsInChildren<PoolableMono>();
        foreach (var obj in childObjs)
        {
            obj.transform.SetParent(null, false);
            PoolManager.Instance.Push(obj);
        }

        string plantTypeName = Enum.GetName(typeof(GrowthState), state);

        foreach (Vector3 pos in plantPositions)
        {
            var plant = PoolManager.Instance.Pop($"{objName}{plantTypeName}");

            SetTransformUtil.SetTransformParent(plant.transform, transform, pos, false);
        }
    }
}
