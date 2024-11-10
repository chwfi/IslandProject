using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : PlaceableObject
{
    private InGameMaterial _harvestMaterial;

    private Animator _animator;

    private readonly int CompleteHash = Animator.StringToHash("Complete");

    private void OnEnable() 
    {
        _animator = transform.Find("Object").GetComponent<Animator>();        
    }

    private void OnComplete()
    {
        if (_harvestMaterial != null)
            return;

        _harvestMaterial = SetRandomMaterial();
    }

    private InGameMaterial SetRandomMaterial()
    {
        List<InGameMaterial> materials = new List<InGameMaterial>();

        foreach (var value in MaterialManager.Instance.MaterialDatabase.StuffMaterials)
        {
            materials.Add(value);
        }

        int randomValue = Random.Range(0, materials.Count);

        var selectedMaterial = materials[randomValue];

        return selectedMaterial;
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
        ShowHarvestUI(_harvestMaterial);
    }

    public override void OnHarvest()
    {
        MaterialManager.Instance.AddMaterialCount(_harvestMaterial, 1);

        ObjectState = PlaceableObjectState.Inactive;
        OnInactive();
        
        _timer = 0;
    }
}
