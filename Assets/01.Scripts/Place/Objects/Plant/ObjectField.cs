using DG.Tweening;
using UnityEngine;
using Util;

[System.Serializable]
public class FieldSaveData
{
    public Vector3 placedPosition;
    public GrowthState growthState;
    public bool isPlaced;
}

public enum GrowthState
{
    Seed,
    Middle,
    Complete
}

public class ObjectField : PlaceableObject
{
    public GrowthState GrowthState { get; private set; }

    private PlantGroup _plantGroup;
    private float _timer;
    private const string PLANT_GROUP_PATH = "PlantGroup";
    private const string HARVEST_BUTTON_PREFAB = "HarvestButtonUI";
    private const string DUST_EFFECT_PREFAB = "DustEffect";

    public override void OnPlace()
    {
        base.OnPlace();
        if (placed)
        {
            InitializeField();
        }
    }

    private void InitializeField()
    {
        _plantGroup = transform.Find(PLANT_GROUP_PATH).GetComponent<PlantGroup>();
        FieldStateManager.Instance.FieldList.Add(this);
        InitPlants();
    }

    private void InitPlants()
    {
        UpdatePlants();
    }

    private void UpdatePlants()
    {
        _plantGroup.SetPlants(GrowthState, _objectData.objectName);
    }

    public void SetTimer(float endTime)
    {
        if (GrowthState == GrowthState.Complete) return;

        _timer += Time.deltaTime;
        if (_timer >= endTime)
        {
            CheckState();
        }
    }

    public void CheckState()
    {
        switch (GrowthState)
        {
            case GrowthState.Seed:
                AdvanceToMiddle();
                break;
            case GrowthState.Middle:
                AdvanceToComplete();
                break;
            case GrowthState.Complete:
                ShowHarvestUI();
                break;
        }
    }

    private void AdvanceToMiddle()
    {
        GrowthState = GrowthState.Middle;
        UpdatePlants();
        ResetTimer();
    }

    private void AdvanceToComplete()
    {
        GrowthState = GrowthState.Complete;
        UpdatePlants();
        ResetTimer();
        ShowHarvestUI();
    }

    private void ResetTimer()
    {
        _timer = 0;
    }

    private void ShowHarvestUI()
    {
        var ui = PoolManager.Instance.Take(HARVEST_BUTTON_PREFAB, transform) as HarvestButtonUI;
        ui.SetObject(this);
    }

    public void OnHarvest()
    {
        MaterialManager.Instance.AddMaterialCount(_objectData.material, _plantGroup.PlantPositions.Length);
        GrowthState = GrowthState.Seed;
        InitPlants();
    }

    public FieldSaveData ToSaveData()
    {
        return new FieldSaveData
        {
            placedPosition = transform.position,
            growthState = GrowthState,
            isPlaced = placed
        };
    }

    public void LoadFrom(FieldSaveData saveData)
    {
        ApplySaveData(saveData);
        SetupPlacedState();
        InitializeField();

        if (saveData.growthState == GrowthState.Complete)
            ShowHarvestUI();
    }

    private void ApplySaveData(FieldSaveData saveData)
    {
        transform.position = saveData.placedPosition;
        GrowthState = saveData.growthState;
        placed = saveData.isPlaced;
    }

    private void SetupPlacedState()
    {
        _modelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.05f);
        SpawnDustEffect();
        DisableUnneededUI();
        FinalizePlacement();
    }

    private void SpawnDustEffect()
    {
        var effect = PoolManager.Instance.Take(DUST_EFFECT_PREFAB, transform) as EffectPlayer;
        SetTransformUtil.SetTransformParent(effect.transform, transform, Vector3.zero, false);
        effect.transform.localScale = new Vector3(3, 3, 3);
    }

    private void DisableUnneededUI()
    {
        _placeableChecker.gameObject.SetActive(false);
        transform.GetComponentInChildren<BuildOptionUI>().gameObject.SetActive(false);
    }

    private void FinalizePlacement()
    {
        PlaceManager.Instance.InitTargetHeight();
        CameraController.Instance.canControll = true;
        placed = true;
    }
}