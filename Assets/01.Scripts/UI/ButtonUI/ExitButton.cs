using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : ButtonUI
{
    protected override void Awake() 
    {
        base.Awake();

        SetSubscriptionSelf(() => OwnerPopup.AccessUI(false));
    }
}
