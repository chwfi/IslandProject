using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private PoolingList _poolingList;
    [SerializeField] private int batchSize = 15; // 한 번에 생성(로딩)할 오브젝트 수

    private Dictionary<string, Stack<IPoolable>> pools = new Dictionary<string, Stack<IPoolable>>();
    private Coroutine loadingCoroutine;

    public bool IsReady => loadingCoroutine == null;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        if (loadingCoroutine == null)
        {
            loadingCoroutine = StartCoroutine(SetUpPools());
        }
    }

    public event System.Action<string> OnPoolListLoadingStarted;
    public event System.Action<string, int, int> OnPoolItemLoaded;

    private IEnumerator SetUpPools()
    {
        var initTasks = new List<IEnumerator>
        {
            SetUpPoolList(_poolingList.MainList, "섬 모델"),
            SetUpPoolList(_poolingList.UIList, "UI"),
            SetUpPoolList(_poolingList.PlaceableObjectList, "건물 오브젝트"),
            SetUpPoolList(_poolingList.EffectList, "이펙트"),
            SetUpPoolList(_poolingList.PlantList, "식물")
        };

        yield return new WaitForSeconds(0.1f);
        
        while (initTasks.Count > 0)
        {
            for (int i = initTasks.Count - 1; i >= 0; i--)
            {
                if (!initTasks[i].MoveNext())
                {
                    initTasks.RemoveAt(i);
                }
                else if (initTasks[i].Current != null)
                {
                    yield return initTasks[i].Current;
                }
            }
            yield return null;
        }

        loadingCoroutine = null;
    }

    private IEnumerator SetUpPoolList(List<PoolingItem> poolList, string listName)
    {
        if (poolList == null || poolList.Count == 0) yield break;

        OnPoolListLoadingStarted?.Invoke(listName);

        // 모든 프리팹을 일단 먼저 비동기로 로드
        var prefabLoadTasks = new Dictionary<PoolingItem, AsyncOperationHandle<GameObject>>();
        foreach (var poolItem in poolList)
        {
            prefabLoadTasks[poolItem] = poolItem.assetReference.LoadAssetAsync<GameObject>();
        }

        while (prefabLoadTasks.Values.Any(task => !task.IsDone))
        {
            yield return null;
        }

        foreach (var poolItem in poolList)
        {
            var loadHandle = prefabLoadTasks[poolItem];
            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Failed to load prefab for {poolItem.assetReference.AssetGUID}");
                continue;
            }

            string assetName = loadHandle.Result.name;
            var newPool = new Stack<IPoolable>(poolItem.poolCount);
            pools[GetOriginalObjectName(assetName)] = newPool;

            for (int i = 0; i < poolItem.poolCount; i += batchSize)
            {
                var batchTasks = new List<AsyncOperationHandle<GameObject>>();
                int remainingCount = Mathf.Min(batchSize, poolItem.poolCount - i);

                for (int j = 0; j < remainingCount; j++)
                {
                    batchTasks.Add(poolItem.assetReference.InstantiateAsync(transform));
                }

                while (batchTasks.Any(task => !task.IsDone))
                {
                    yield return null;
                }

                foreach (var handle in batchTasks)
                {
                    var newGameObject = handle.Result;
                    if (!newGameObject.TryGetComponent<IPoolable>(out var poolable))
                    {
                        Debug.LogError("Poolable object must implement IPoolable interface.");
                        Addressables.ReleaseInstance(newGameObject);
                        continue;
                    }

                    newGameObject.SetActive(false);
                    newPool.Push(poolable);
                    OnPoolItemLoaded?.Invoke(assetName, newPool.Count, poolItem.poolCount);
                }

                yield return new WaitForSeconds(0.01f); // 프레임 드랍 방지를 위한 짧은 대기
            }

            // 원본 프리팹 에셋 해제
            Addressables.Release(loadHandle);
        }
    }

    public IPoolable Take(string objectName, Transform parent)
    {
        if (!IsReady) return null;

        if (!pools.TryGetValue(objectName, out var pool))
        {
            Debug.LogError($"Pool for GameObject {objectName} doesn't exist!");
            return null;
        }

        if (pool.Count > 0)
        {
            var newObject = pool.Pop();
            var monoBehaviour = newObject as MonoBehaviour;

            monoBehaviour.transform.SetParent(parent, false);
            monoBehaviour.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            monoBehaviour.gameObject.SetActive(true);
            newObject.OnTakenFromPool();

            return newObject;
        }

        Debug.LogWarning($"Pool for {objectName} is empty!");
        return null;
    }

    public void Return(IPoolable poolableObject)
    {
        var monoBehaviour = poolableObject as MonoBehaviour;
        string objectName = GetOriginalObjectName(monoBehaviour.name);

        if (pools.TryGetValue(objectName, out var pool))
        {
            poolableObject.OnReturnedToPool();
            monoBehaviour.gameObject.SetActive(false);
            monoBehaviour.transform.SetParent(transform);
            pool.Push(poolableObject);
        }
        else
        {
            Debug.LogWarning($"No pool exists for object {objectName}");
            Addressables.ReleaseInstance(monoBehaviour.gameObject);  // 풀에 없으면 Addressable에서 해제
        }
    }

    private string GetOriginalObjectName(string cloneName)
    {
        return cloneName.Replace("(Clone)", "").Trim();
    }
}