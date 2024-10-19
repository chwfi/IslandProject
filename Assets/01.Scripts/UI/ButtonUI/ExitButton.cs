using UnityEngine;

public class ExitButton : ButtonUI
{
    [SerializeField] private bool _moveExit;
    [SerializeField] private Vector3 _moveDirection;

    protected override void Awake() 
    {
        base.Awake();

        if (_moveExit)
            SetSubscriptionSelf(() => OwnerPopup.MoveUI(_moveDirection));
        else
            SetSubscriptionSelf(() => OwnerPopup.AccessUI(false));
    }
}
