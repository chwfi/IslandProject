public class RepositoryBuilding : BaseBuilding
{
    private void OnMouseDown()
    {   
        if (!GameManager.Instance.IsPointerOverUIObject())
        {
            PopupUIManager.Instance.AccessPopupUI("MaterialRepositoryPanel", true);
        }
    }
}
