using UnityEngine;

public class TestObj : MonoBehaviour, IPoolable
{
    [SerializeField] PoolTypeSO _poolType;
    public PoolTypeSO PoolType => _poolType;

    public GameObject GameObject => gameObject;

    private M_Pool _myPool;

    public void ResetItem()
    {
        
    }

    public void SetUpPool(M_Pool pool)
    {
        _myPool = pool;    
    }
}
