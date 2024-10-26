using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
  
public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private PoolingList _poolingList;

    private Dictionary<string, Stack<IPoolable>> pools = new Dictionary<string, Stack<IPoolable>>();
    private Stack<IPoolable> pool = null;
    public bool IsReady { get { return loadingCoroutine == null; } }
    private Coroutine loadingCoroutine;

    public IPoolable Take(string objectName, Transform parent)
    {
        if (!IsReady) return null;
        
        if (!pools.ContainsKey(objectName))
        {
            Debug.LogError($"Pool for GameObject {objectName} doesn't exist!");
            return null;
        }

        var pool = pools[objectName];
        if (pool.Count > 0)
        {
            var newObject = pool.Pop();

            (newObject as MonoBehaviour).transform.SetParent(parent, false);
            (newObject as MonoBehaviour).gameObject.SetActive(true);

            newObject.OnTakenFromPool();

            return newObject;
        }

        Debug.LogWarning($"Pool for {objectName} is empty!");
        return null;
    }

    public void Return(IPoolable poolableObject)
    {
        poolableObject.OnReturnedToPool();
        (poolableObject as MonoBehaviour).gameObject.SetActive(false);
        (poolableObject as MonoBehaviour).transform.parent = transform;
        pool.Push(poolableObject);
    }

    void Awake()
    {
        //allAvailablePools[assetReferenceToInstantiate.RuntimeKey] = this;
        loadingCoroutine = StartCoroutine(SetupPools());
    }

    void OnDisable()
    {
        //allAvailablePools.Remove(assetReferenceToInstantiate.RuntimeKey);
        foreach (var obj in pool)
        {
            Addressables.ReleaseInstance((obj as MonoBehaviour).gameObject);
        }
        pool = null;
    }

    private IEnumerator SetupPools()
    {
        foreach (var pooling in _poolingList.PoolList)
        {
            var handle = pooling.assetReference.InstantiateAsync(transform);
            yield return handle;
            
            // 프리팹의 이름을 가져오고 (Clone) 제거
            var tempObject = handle.Result;
            string objectName = GetOriginalObjectName(tempObject.name);
            Addressables.ReleaseInstance(tempObject);

            var newPool = new Stack<IPoolable>(pooling.poolCount);
            pools[objectName] = newPool;

            for (var i = 0; i < pooling.poolCount; i++)
            {
                handle = pooling.assetReference.InstantiateAsync(transform);
                yield return handle;
                var newGameObject = handle.Result;
                var poolable = newGameObject.GetComponent<IPoolable>();
                    
                if (poolable == null)
                {
                    Debug.LogError($"Poolable object {objectName} must implement IPoolable interface.");
                    Addressables.ReleaseInstance(newGameObject);
                    continue;
                }

                newPool.Push(poolable);
                newGameObject.SetActive(false);
            }
        }

        loadingCoroutine = null;
    }

    private string GetOriginalObjectName(string cloneName)
    {
        return cloneName.Replace("(Clone)", "").Trim();
    }
}
