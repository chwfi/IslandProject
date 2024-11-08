using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HarvestButtonUI : WorldUI, IDragInteractable
{
    [SerializeField] private Image _buttonImage;

    private PlaceableObject _ownObject;

    public void OnInteract()
    {
        _ownObject.OnHarvest();
        var effect = PoolManager.Instance.Take("BubbleMuzzleEffect", null) as EffectPlayer;
        effect.transform.localPosition = transform.position;
        effect.transform.localScale = new Vector3(2, 2, 2);
        _ownObject = null;
        
        PoolManager.Instance.Return(this);
    }

    public void SetObject(PlaceableObject obj, object material)
    {
        transform.GetComponent<RectTransform>().DOAnchorPosY(8.3f, 0.9f).SetEase(Ease.OutBack);

        //_buttonImage.sprite = material.Icon;
        _ownObject = obj;
    }
}