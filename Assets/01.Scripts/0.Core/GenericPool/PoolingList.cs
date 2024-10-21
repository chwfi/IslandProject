using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingPair
{
    public PoolableMono prefab;
    public int poolCount;
}

[CreateAssetMenu(menuName = "SO/Pool/list")]  
public class PoolingList : ScriptableObject
{
    public List<PoolingPair> PoolingPairs;
    public List<PoolingPair> PlaceableObjects;
}