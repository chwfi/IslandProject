using DG.Tweening;
using UnityEngine;

public class PlaceableObject : PoolableMono
{
    private PlaceableObjectData _objectData;

    private BuildOptionUI _buildOptionUI;
    private PlaceableChecker _placeableChecker;

    private void Awake() 
    {
        _placeableChecker = transform.GetComponentInChildren<PlaceableChecker>();    
    }

    public void SetPlaceableObject(PlaceableObjectData data) 
    {
        _objectData = data;

        _buildOptionUI = transform.GetComponentInChildren<BuildOptionUI>();
        _buildOptionUI.SetObject(this);

        if (!_placeableChecker.gameObject.activeInHierarchy)
        {
            _placeableChecker.gameObject.SetActive(true);
        }
    }

    public virtual void OnPlace()
    {
        if (!_placeableChecker.CanPlace) return;

        ItemManager.Instance.UseCoin(_objectData.price, () => 
        {
            transform.DOMoveY(_placeableChecker.transform.position.y, 0.05f).OnComplete(() =>
            {
                var effect = PoolManager.Instance.Pop("DustEffect");
                effect.transform.position = transform.position;
            });
            _placeableChecker.gameObject.SetActive(false);
            _buildOptionUI.gameObject.SetActive(false);

            CameraController.Instance.canControll = true;
        }, 
        () => 
        {
            PoolManager.Instance.Push(this);
        });
    }
}
