using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour, IPoolable
{
    [SerializeField] PoolTypeSO _poolType;
    public PoolTypeSO PoolType => _poolType;

    public GameObject GameObject => gameObject;

    public void ResetItem()
    {

    }

    public void SetUpPool(M_Pool pool)
    {
        
    }
}
