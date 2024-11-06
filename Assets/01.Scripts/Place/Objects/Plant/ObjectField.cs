using DG.Tweening;
using UnityEngine;
using Util;

public class ObjectField : PlaceableObject
{
    public Vector3[] GetPlantPositions()
    {
        return transform.Find("PlantGroup").GetComponent<PlantGroup>().PlantPositions;
    }

    private void InitializeField()
    {
        ObjectStateManager.Instance.ObjectList.Add(this);
        InitPlants();
    }

    private void InitPlants()
    {
        UpdatePlants();
    }

    private void UpdatePlants()
    {

    }

    public override void CheckState()
    {
        
    }

    private void SpawnDustEffect()
    {
        // var effect = PoolManager.Instance.Take(DUST_EFFECT_PREFAB, transform) as EffectPlayer;
        // SetTransformUtil.SetTransformParent(effect.transform, transform, Vector3.zero, false);
        // effect.transform.localScale = new Vector3(3, 3, 3);
    }
}