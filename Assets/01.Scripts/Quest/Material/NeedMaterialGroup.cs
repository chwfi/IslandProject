public enum MaterialState
{
    Inactive,
    Active,
    Complete
}

[System.Serializable]
public class NeedMaterialGroup
{
    public InGameMaterial material;
    public int needAmount;

    private MaterialState _materialState;
    public MaterialState MaterialState => _materialState;
    public bool IsComplete => MaterialState == MaterialState.Complete;

    public Quest Owner { get; private set; }
     
    public void SetOwner(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        _materialState = MaterialState.Active;
    }

    public void Complete()
    {
        _materialState = MaterialState.Complete;
    }
}