using DG.Tweening;
using UnityEngine;

public class QuestPanel : PopupUI
{
    [Header("Child UI Transforms")]
    [SerializeField] private RectTransform _sideListUI;
    [SerializeField] private RectTransform _middleInfoUI;
    [SerializeField] private RectTransform _upperTitleUI;

    public override void Awake() 
    {
        base.Awake();

        InactiveUIElements();   
    }

    public override void AccessUI(bool active)
    {
        base.AccessUI(active);

        if (active)
        {
            ActiveUIElements();
        }
        else
        {
            InactiveUIElements();
        }
    }

    private void ActiveUIElements()
    {
        _sideListUI.DOLocalRotate(Vector3.zero, 0.8f).SetEase(Ease.InOutBack);
        _middleInfoUI.DOAnchorPosY(-63, 0.8f).SetEase(Ease.OutQuart);
        _upperTitleUI.DOAnchorPosY(-46, 0.8f).SetEase(Ease.InOutBack).
        OnComplete(() => 
        {
            _buttonList.ForEach(x => x.gameObject.SetActive(true));
        });
    }

    private void InactiveUIElements()
    {
        _buttonList.ForEach(x => x.gameObject.SetActive(false));

        _sideListUI.DOLocalRotate(new Vector3(0, 0, 73), 0.6f).SetEase(Ease.InOutBack);
        _middleInfoUI.DOAnchorPosY(-750, 0.6f).SetEase(Ease.OutQuart);
        _upperTitleUI.DOAnchorPosY(250, 0.6f).SetEase(Ease.InOutBack);
    }
}
