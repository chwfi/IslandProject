using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : PlaceableObject
{
    private Animator _animator;

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int WorkHash = Animator.StringToHash("Work");
    private readonly int CompleteHash = Animator.StringToHash("Complete");

    private void Start() 
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
    }

    public override void OnInactive()
    {
        _animator.SetBool(IdleHash, true);
    }

    public override void OnActive()
    {
        _animator.SetBool(WorkHash, true);
    }

    public override void OnWaitForCompletion()
    {
        _animator.SetBool(CompleteHash, true);

        OnComplete();
        ShowHarvestUI();
    }
}
