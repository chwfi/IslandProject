using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : PlaceableObject
{
    private Animator _animator;

    public override void CheckState()
    {
        throw new System.NotImplementedException();
    }

    private void Awake() 
    {
        _animator = transform.Find("Object").GetComponent<Animator>();    
    }

    private void OnComplete()
    {
        List<InGameMaterial> materials = new List<InGameMaterial>();

        foreach (var value in MaterialManager.Instance.MaterialDatabase.StuffMaterials)
        {
            materials.Add(value);
        }

        int randomIndex = Random.Range(0, materials.Count);

        ObjectData.material = materials[randomIndex];

        ShowHarvestUI();
    }
}
