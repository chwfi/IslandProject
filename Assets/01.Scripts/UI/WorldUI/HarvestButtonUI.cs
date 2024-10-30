using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HarvestButtonUI : WorldUI
{
    [SerializeField] private Button _harvestButton;

    public void SetObject(ObjectField obj)
    {
        transform.GetComponent<RectTransform>().DOAnchorPosY(8.3f, 0.9f).SetEase(Ease.OutBack);

        _harvestButton.onClick.AddListener(() => 
        {
            obj.OnHarvest();
            _harvestButton.onClick.RemoveAllListeners(); 
            var effect = PoolManager.Instance.Take("BubbleMuzzleEffect", null) as EffectPlayer;
            effect.transform.localPosition = transform.position;
            effect.transform.localScale = Vector3.one;
            PoolManager.Instance.Return(this);
        });
    }
}