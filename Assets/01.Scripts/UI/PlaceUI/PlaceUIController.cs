using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlaceUIController : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private StructureUI _structureUIPrefab;

    [Header("UI Transform")]
    [SerializeField] private Transform _horizontalGroup;

    private void Start() 
    {
        PlaceManager manager = PlaceManager.Instance;

        foreach (var placeableData in manager.PlaceableDatabase.PlaceableObjects)
        {
            var ui = PoolManager.Instance.Pop(_structureUIPrefab.name) as StructureUI;
            SetTransformUtil.SetUIParent(ui.transform, _horizontalGroup, Vector3.zero, false);
            ui.SetUI(placeableData);
        }  
    }
}
