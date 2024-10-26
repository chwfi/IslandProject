using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IPoolable
{
    public void OnReturnedToPool()
    {
        transform.SetParent(null, false);
    }

    public void OnTakenFromPool()
    {

    }
}
