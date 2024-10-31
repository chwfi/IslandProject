using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private Transform _instantiateTrm;

    private void Awake() 
    {
        foreach (var item in PoolManager.Instance.PoolingList.EnvironmentList)
        {
            PoolManager.Instance.Take(item.assetReference.Asset.name, _instantiateTrm);
        }
        PoolManager.Instance.Take("Zone", _instantiateTrm);
    }
}