public class HomeBuilding : BaseBuilding
{
    private void OnMouseDown()
    {   
        if (IsClickable())
        {
            PopupUIManager.Instance.AccessPopupUI("QuestPanel", true);
        }
    }
}
