using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private PoolingList _poolingList;

    private void Awake()
    {
        MakePool();

        //Application.targetFrameRate = 60;
    }

    private void MakePool()
    {
        PoolManager.Instance = new PoolManager(transform);

        _poolingList.PoolingPairs.ForEach(p => PoolManager.Instance.CreatePool(p.prefab, p.poolCount));
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuestSystem.Instance.Report("test", 1);
        }    
    }
}