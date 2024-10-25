using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;

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
            var effect = PoolManager.Instance.Pop("BubbleMuzzleEffect");
            effect.transform.position = transform.position;
            PoolManager.Instance.Push(this);
        });
    }
}