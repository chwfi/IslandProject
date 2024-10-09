using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnboundedTaskUI : UnboundedUI
{
    [HideInInspector]
    public Task OwnTask;
    
    public override void UpdateUI()
    {
        _amountText.text = $"{OwnTask.Description}\n{OwnTask.CurrentSuccessValue}/{OwnTask.NeedToSuccessValue}";
    }
}
