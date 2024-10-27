public class HomeBuilding : BaseBuilding
{
    private void OnMouseDown()
    {   
        if (!GameManager.Instance.IsPointerOverUIObject())
        {
            PopupUIManager.Instance.AccessPopupUI("QuestPanel", true);
        }
    }
}
