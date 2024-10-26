using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private InGameMaterial test;

    [HideInInspector] public Camera MainCam;

    private void Awake()
    {
        MakePool();

        MainCam = Camera.main;
    }

    private void MakePool()
    {  
        //PoolManager.Instance.CreatePoolFromAddressable(_poolingList);
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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