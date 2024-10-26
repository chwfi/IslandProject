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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T))
        {
            test();
        }
    }

    private void test() 
    {
        PlaceManager manager = PlaceManager.Instance;

        foreach (var placeableData in manager.PlaceableDatabase.PlaceableObjects)
        {
            var ui = PoolManager.Instance.Take(_structureUIPrefab.name, _horizontalGroup) as StructureUI;
            //SetTransformUtil.SetUIParent(ui.transform, _horizontalGroup, Vector3.zero, false);
            ui.SetUI(placeableData);
            Debug.Log(ui);
        }  
    }
}
