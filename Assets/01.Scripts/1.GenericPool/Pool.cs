using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pool<T> where T : PoolableMono
{
    private Stack<T> _pool = new Stack<T>();
    private T _prefab;
    private Transform _parent;

    public Pool(T prefab, Transform parent, int count)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(prefab, _parent.position, _parent.rotation);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }

    public T Pop()
    {
        T obj = null;

        if (_pool.Count <= 0)
        {
            obj = GameObject.Instantiate(_prefab, _parent.position, _parent.rotation);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");

        }
        else
        {
            obj = _pool.Pop();
            obj.gameObject.SetActive(true);
        }

        return obj;
    }

    public void Push(T obj)
    {
        obj.transform.parent = null;
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }
}
