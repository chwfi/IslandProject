using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private PoolingList _poolingList;

    [SerializeField]
    private InGameMaterial test;

    [HideInInspector] public Camera MainCam;

    private void Awake()
    {
        MakePool();

        MainCam = Camera.main;
        //Application.targetFrameRate = 60;
    }

    private void MakePool()
    {
        PoolManager.Instance = new PoolManager(transform);

        _poolingList.PoolingPairs.ForEach(p => PoolManager.Instance.CreatePool(p.prefab, p.poolCount));
        _poolingList.PlaceableObjects.ForEach(p => PoolManager.Instance.CreatePool(p.prefab, p.poolCount));
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuestManager.Instance.Report("test", 1);
        }    

        if (Input.GetKeyDown(KeyCode.G))
        {
            MaterialManager.Instance.AddMaterialCount(test, 1);
        }
    }
}