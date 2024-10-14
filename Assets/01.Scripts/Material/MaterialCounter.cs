public class MaterialCounter
{
    public MaterialCounter(int count)
    {
        materialCount = count;
    }

    public int materialCount;

    public void ReceieveReport(int amount)
    {
        materialCount += amount;
    }
}
