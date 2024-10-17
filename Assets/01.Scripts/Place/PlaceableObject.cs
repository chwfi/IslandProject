using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Util;

public class PlaceableObject : PoolableMono
{
    [SerializeField] private int _price;

    private BuildOptionUI _buildOptionUI;

    private PlaceableChecker _placeableChecker;

    private void Awake() 
    {
        _placeableChecker = transform.GetComponentInChildren<PlaceableChecker>();    
    }

    public void SetPlaceableObject() 
    {
        _buildOptionUI = PoolManager.Instance.Pop("BuildOptionUI") as BuildOptionUI;
        SetTransformUtil.SetUIParent(_buildOptionUI.transform, transform, new Vector3(0, 4, -1), false);
        _buildOptionUI.SetObject(this);

        if (!_placeableChecker.gameObject.activeInHierarchy)
        {
            _placeableChecker.gameObject.SetActive(true);
        }
    }

    public void OnPlace()
    {
        if (!_placeableChecker.CanPlace) return;

        ItemManager.Instance.UseCoin(_price, () => 
        {
            transform.DOMoveY(_placeableChecker.transform.position.y, 0.05f).OnComplete(() =>
            {
                var effect = PoolManager.Instance.Pop("DustEffect");
                effect.transform.position = transform.position;
            });
            _placeableChecker.gameObject.SetActive(false);
            PoolManager.Instance.Push(_buildOptionUI);
        }, 
        () => 
        {
            PoolManager.Instance.Push(this);
        });
    }
}
