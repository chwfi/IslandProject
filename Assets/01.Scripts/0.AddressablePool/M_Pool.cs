using System;
using System.Collections.Generic;
using UnityEngine;

public class M_Pool  
{
    private Stack<IPoolable> _pool;
    private Transform _parent;
    private PoolTypeSO _poolType;
    public event Action LoadCompleteEvent;

    public M_Pool(PoolTypeSO poolType, Transform parent, int count)
    {
        _pool = new Stack<IPoolable>(count);
        _parent = parent;
        _poolType = poolType;
        
        LoadAndInstantiate(count);
    }

    private async void LoadAndInstantiate(int count)
    {
        var asset = _poolType.assetRef;
        await asset.LoadAssetAsync<GameObject>().Task;
        //로드 실패시 에러처리
        Debug.Assert(asset.IsValid(),  $"Error : Loading asset failed {_poolType.typeName}");

        for (int i = 0; i < count; i++)
        {
            GameObject gameObj = await asset.InstantiateAsync().Task;
            gameObj.SetActive(false);
            gameObj.transform.SetParent(_parent);
            IPoolable item = gameObj.GetComponent<IPoolable>();
            item.SetUpPool(this);
            _pool.Push(item);
        }

        LoadCompleteEvent?.Invoke(); //로딩성공 메시지
    }


    public IPoolable Pop()
    {
        IPoolable item;
        if (_pool.Count == 0)
        {

            GameObject gameObj = GameObject.Instantiate(_poolType.assetRef.Asset, _parent) as GameObject;
            item = gameObj.GetComponent<IPoolable>();
            item.SetUpPool(this);
        }
        else
        {
            item = _pool.Pop();
            item.GameObject.SetActive(true);
        }
        item.ResetItem();
        return item;
    }

    public void Push(IPoolable item)
    {
        item.GameObject.SetActive(false);
        _pool.Push(item);
    }
}
