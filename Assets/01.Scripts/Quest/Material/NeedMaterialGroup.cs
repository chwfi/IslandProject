[System.Serializable]
public class NeedMaterialGroup
{
    public InGameMaterial material;
    public int needAmount;

    public void FindMaterial()
    {
        var newMaterial = MaterialManager.Instance.FindQuestBy(material.Icon);
        material = newMaterial;
    }
}