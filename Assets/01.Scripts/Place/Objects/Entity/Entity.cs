using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : PlaceableObject
{
    private Animator _animator;

    private readonly int CompleteHash = Animator.StringToHash("Complete");

    private void OnEnable() 
    {
        _animator = transform.Find("Object").GetComponent<Animator>();        
    }

    private void OnComplete()
    {
        if (_harvestObject != null)
            return;

        List<InGameMaterial> materials = new List<InGameMaterial>();

        foreach (var value in MaterialManager.Instance.MaterialDatabase.StuffMaterials)
        {
            materials.Add(value);
        }

        int randomIndex = Random.Range(0, materials.Count);

        _harvestObject = materials[randomIndex];
    }

    public override void OnInactive()
    {
        _animator.SetBool(CompleteHash, false);
    }

    public override void OnActive()
    {

    }

    public override void OnWaitForCompletion()
    {
        _animator.SetBool(CompleteHash, true);

        OnComplete();
        ShowHarvestUI(_harvestObject);
    }
}
