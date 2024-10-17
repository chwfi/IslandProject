public class ExitButton : ButtonUI
{
    protected override void Awake() 
    {
        base.Awake();

        SetSubscriptionSelf(() => OwnerPopup.AccessUI(false));
    }
}
