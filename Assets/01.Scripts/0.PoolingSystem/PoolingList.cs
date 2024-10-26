using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Util;

[System.Serializable]
public class PoolingItem
{
    public int poolCount;
    public AssetReference assetReference;
}

[CreateAssetMenu(menuName = "SO/PoolingList")]
public class PoolingList : ScriptableObject
{
    [SerializeField] private List<PoolingItem> _poolList;
    public List<PoolingItem> PoolList => _poolList;
}
