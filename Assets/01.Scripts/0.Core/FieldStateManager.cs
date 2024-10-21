using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStateManager : MonoSingleton<FieldStateManager>
{
    [SerializeField] private float _growthEndTime;

    public List<ObjectField> FieldList { get; set; }

    private void Awake() 
    {
        FieldList = new List<ObjectField>();    
    }

    private void Update() 
    {
        if (FieldList.Count <= 0)
            return;

        foreach (var field in FieldList)
        {
            field.SetTimer(_growthEndTime);
        }
    }
}
