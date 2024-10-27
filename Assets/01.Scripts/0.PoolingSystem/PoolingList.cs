using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class PoolingItem
{
    public int poolCount;
    public AssetReference assetReference;
}  

[CreateAssetMenu(menuName = "SO/PoolingList")]
public class PoolingList : ScriptableObject
{
    [SerializeField] private List<PoolingItem> _mainList;
    [SerializeField] private List<PoolingItem> _uiList;
    [SerializeField] private List<PoolingItem> _placeableObjectList;
    [SerializeField] private List<PoolingItem> _effectList;
    [SerializeField] private List<PoolingItem> _plantList;

    public List<PoolingItem> MainList => _mainList;
    public List<PoolingItem> UIList => _uiList;
    public List<PoolingItem> PlaceableObjectList => _placeableObjectList;
    public List<PoolingItem> EffectList => _effectList;
    public List<PoolingItem> PlantList => _plantList;
}
