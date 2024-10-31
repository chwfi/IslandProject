using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HarvestButtonUI : WorldUI, IDragInteractable
{
    [SerializeField] private Button _harvestButton;

    private ObjectField _ownField;

    public void OnInteract()
    {
        _ownField.OnHarvest();
        var effect = PoolManager.Instance.Take("BubbleMuzzleEffect", null) as EffectPlayer;
        effect.transform.localPosition = transform.position;
        effect.transform.localScale = Vector3.one;
        _ownField = null;
        
        PoolManager.Instance.Return(this);
    }

    public void SetObject(ObjectField obj)
    {
        transform.GetComponent<RectTransform>().DOAnchorPosY(8.3f, 0.9f).SetEase(Ease.OutBack);
        _ownField = obj;
    }
}