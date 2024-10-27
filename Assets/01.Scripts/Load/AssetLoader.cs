using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    private void Awake() 
    {
        PoolManager.Instance.Take("Environment", null);    
        PoolManager.Instance.Take("Zone", null);
    }
}