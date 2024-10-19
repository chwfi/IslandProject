using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using DG.Tweening;
using System;

public class PopupUI : PoolableMono
{
    [Header("Fade Value")]
    [SerializeField] private float _fadeDealy = 0f; // UI가 켜질때 바로 켜지지 않고 딜레이를 줄것인가
    [SerializeField] private float _fadeTime; // UI가 켜지거나 꺼질 때 몇초동안 페이드를 줄것인가
    [SerializeField] private float _disableDelay; // 꺼지기까지 몇초

    [Header("Move Value")]
    [SerializeField] private float _moveDuration;

    [Header("Option")]
    [SerializeField] private bool _activeOnStart; // 시작했을 때 켜줄것인가
    [SerializeField] private bool _autoDisable; // 켜진 후 꺼지게 할것인가

    protected CanvasGroup _canvasGroup; // 팝업 UI는 캔버스 그룹으로 관리한다
    protected List<ButtonUI> _buttonList = new(); // UI에 달려있는 버튼들의 리스트

    public virtual void Awake()
    {
        if (TryGetComponent(out CanvasGroup canvasGroup)) // 캔버스 그룹 가져오기
            _canvasGroup = canvasGroup;

        var buttons = transform.GetComponentsInChildren<ButtonUI>(); // UI에 달려있는 버튼들 가져오기

        if (buttons.Any()) 
        {
            _buttonList.AddRange(buttons);
        }
    }

    public virtual void AccessUI(bool active) // UI에 접근하여 키거나 끌 수 있는 함수
    {
        SetInteractive(active);

        if (active) // 켜졌을 때
        {
            transform.SetAsLastSibling(); // 자식 순서를 가장 나중으로 하여 맨 앞에 띄워지게 한다.
            
            if (_autoDisable) // 자동 비활성화가 켜져있다면, disableDelay만큼 기다렸다가 UI 꺼줌
            {
                CoroutineUtil.CallWaitForSeconds(_disableDelay, () => AccessUI(false));
            }
        }

        CoroutineUtil.CallWaitForSeconds(_fadeDealy, () => // 코루틴 유틸 클래스를 이용하여 딜레이 시간만큼 기다려주고,
        {
            _canvasGroup.DOFade(active ? 1f : 0f, _fadeTime); // 캔버스 그룹을 active bool값에 따라 키거나 꺼준다. fade 가능 
        });
    }

    public virtual void MoveUI(Vector3 pos, Action callback = null)
    {
        transform.GetComponent<RectTransform>().DOAnchorPos(pos, _moveDuration).OnComplete(() => callback?.Invoke());     
    }

    private void SetInteractive(bool value)
    {
        _canvasGroup.blocksRaycasts = value;
        _canvasGroup.interactable = value;
    }
}
