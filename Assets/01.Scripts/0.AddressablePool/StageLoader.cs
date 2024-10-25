using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private AssetDBSO _assetDB;
    [SerializeField] private AssetReference _stageAsset;
    
    private GameObject _stage;
    void Start()
    {
        var prefab = _assetDB.GetAsset<GameObject>(_stageAsset.AssetGUID);
        _stage = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    private void OnDestroy() {
        var assetRef = _assetDB.GetAssetReference(_stageAsset.AssetGUID);
        assetRef.ReleaseInstance(_stage);
    }
}
