using UnityEngine;

public class MaterialSaveData
{
    public int codeName;
    public int count;
}

[CreateAssetMenu(menuName = "SO/Material/InGameMaterial", fileName = "Material_")]
public class InGameMaterial : ScriptableObject, ICloneable<InGameMaterial>
{
    [SerializeField] private int _codeName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _materialName;
    [SerializeField] private string _description;
    
    public int CodeName => _codeName;
    public Sprite Icon => _icon;
    public string MaterialName => _materialName;
    public string Description => _description;

    private MaterialCounter _materialCounter;
    public MaterialCounter MaterialCounter => _materialCounter;

    public InGameMaterial Clone()
    {
        var clone = Instantiate(this);

        var materialManager = MaterialManager.Instance;
        materialManager.OnMaterialRecieved += clone.ReceieveReport;

        return clone;
    }

    public bool IsTargetEqual(InGameMaterial target)
    {
        if (_materialName == target.MaterialName)
            return true;
        else
            return false;
    }

    public void ReceieveReport(InGameMaterial target, int amount)
    {
        if (!IsTargetEqual(target)) return;

        _materialCounter.ReceieveReport(amount);
        Debug.Log(amount);
    }

    public MaterialSaveData ToSaveData()
    {
        return new MaterialSaveData
        {
            codeName = _codeName,
            count = _materialCounter.materialCount
        };
    }

    public MaterialSaveData ToInitialSaveData()
    {
        return new MaterialSaveData
        {
            codeName = _codeName,
            count = 0
        };
    }

    public void LoadFrom(MaterialSaveData saveData)
    {
        _codeName = saveData.codeName;
        _materialCounter = new MaterialCounter(saveData.count);
    }
}