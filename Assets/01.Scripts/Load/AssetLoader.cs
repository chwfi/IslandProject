using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.IO;  // System.IO 네임스페이스 추가

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private Transform _instantiateTrm;

    public AssetLabelReference assetLabel;
 
    private IList<IResourceLocation> _locations;

    private void Awake() 
    {
        GetLocations(); // 라벨로 로드하는 기능
    }
 
    public void GetLocations()
    {
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
                Debug.Log($"Found {_locations.Count} assets with label '{assetLabel.labelString}'");

                foreach (var location in _locations)
                {
                    // 파일 경로이름은 제거하고 순수 이름만
                    string assetName = Path.GetFileNameWithoutExtension(location.PrimaryKey);

                    PoolManager.Instance.Take(assetName, _instantiateTrm); // 풀매니저에서 이름을 탐색해 가져옴
                }
            };
    }
}