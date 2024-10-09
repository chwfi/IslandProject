using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

public abstract class ButtonUI : MonoBehaviour, IPointerClickHandler
{
    protected Button _button;
    protected event Action _buttonEvent;

    protected PopupUI _ownerPopup;

    public PopupUI OwnerPopup
    {
        get
        {
            if (_ownerPopup == null)
                Debug.LogWarning("OwnerPopup is null");
            return _ownerPopup;
        }

        private set
        {
            _ownerPopup = value;
        }
    }

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _ownerPopup = FindObjectUtil.FindParent<PopupUI>(this.gameObject);
    }

    public void SetSubscription<T>(Action<T> action)
    {
        _buttonEvent += () => action(default);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartEvent();
    }

    public void StartEvent()
    {
        _buttonEvent?.Invoke();
    }
}
