using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Util;

[CreateAssetMenu(menuName = "SO/AssetDB")]
public class AssetDBSO : ScriptableObject
{
    public List<AssetReference> gameObjectAsset;
    public List<AssetReferenceT<AudioClip>> audioAssets;

    private Dictionary<string, AssetReference> _db;

    public event Action<int> LoadCountEvent;
    public event Action<int, string> LoadMessageEvent;

    public void Initialize()
    {
        CoroutineUtil.CallCoroutine(LoadNonPoolingAssetsCoroutine());
    }

    private IEnumerator LoadNonPoolingAssetsCoroutine()
    {
        _db = new Dictionary<string, AssetReference>();

        LoadCountEvent?.Invoke(gameObjectAsset.Count);
        LoadCountEvent?.Invoke(audioAssets.Count);

        // GameObject 자산 로드
        foreach (var asset in gameObjectAsset)
        {
            var handle = asset.LoadAssetAsync<GameObject>();
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to load GameObject asset: {asset.AssetGUID}");
            }
        }
        LoadMessageEvent?.Invoke(gameObjectAsset.Count, "GameObject asset Loaded");

        // AudioClip 자산 로드
        foreach (var asset in audioAssets)
        {
            var handle = asset.LoadAssetAsync<AudioClip>();
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to load Audio asset: {asset.AssetGUID}");
            }
        }
        LoadMessageEvent?.Invoke(audioAssets.Count, "Audio asset Loaded");

        // 로드된 자산을 사전에 추가
        gameObjectAsset.ForEach(x => _db.Add(x.AssetGUID, x));
        audioAssets.ForEach(x => _db.Add(x.AssetGUID, x));
    }

    public T GetAsset<T>(string guid) where T : UnityEngine.Object
    {
        if(_db.TryGetValue(guid, out AssetReference assetRef))
        {
            return assetRef.Asset as T;
        }
        return default;
    }


    public AssetReference GetAssetReference(string guid)
    {
        if(_db.TryGetValue(guid, out AssetReference assetRef))
        {
            return assetRef;
        }

        return null;
    }
}