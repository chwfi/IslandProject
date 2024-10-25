using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class PoolingPair
{
    public AssetReference assetRef;
    public PoolableMono prefab;
    public int poolCount;
}

[CreateAssetMenu(menuName = "SO/Pool/list")]  
public class PoolingList : ScriptableObject
{
    public List<PoolingPair> PoolingPairs;
    public List<PoolingPair> UI;
    public List<PoolingPair> Effects;
    public List<PoolingPair> PlaceableObjects;
    public List<PoolingPair> Plants;
}