using UnityEngine;

public interface IPoolable
{
    public PoolTypeSO PoolType {get;}
    public GameObject GameObject {get;}
    public void SetUpPool(M_Pool pool);
    public void ResetItem();
}