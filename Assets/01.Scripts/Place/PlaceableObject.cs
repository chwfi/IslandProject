using DG.Tweening;
using UnityEngine;
using Util;

public class PlaceableObject : MonoBehaviour, IPoolable
{
    [SerializeField]
    protected PlaceableObjectData _objectData;

    protected BuildOptionUI _buildOptionUI;
    protected PlaceableChecker _placeableChecker;
    protected GameObject _modelObject;
    public GameObject ModelObject => _modelObject;
    public PlaceableObjectData ObjectData => _objectData;

    protected bool placed = false;

    private void Awake() 
    {
        _placeableChecker = transform.GetComponentInChildren<PlaceableChecker>();   
        _modelObject = transform.Find("Object").gameObject; 
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
            _modelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.05f);
            var effect = PoolManager.Instance.Take("DustEffect", transform) as EffectPlayer;

            SetTransformUtil.SetTransformParent(effect.transform, transform, Vector3.zero, false);
            effect.transform.localScale = new Vector3(3, 3, 3);

            _placeableChecker.gameObject.SetActive(false);
            _buildOptionUI.gameObject.SetActive(false);
            
            PlaceManager.Instance.InitTargetHeight();

            CameraController.Instance.canControll = true;
            placed = true;
        }, 
        () => 
        {
            PoolManager.Instance.Return(this);
        });
    }

    public virtual void OnTakenFromPool()
    {
        if (placed)
        {
            ModelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.01f);
            _placeableChecker.gameObject.SetActive(false);
            _buildOptionUI.gameObject.SetActive(false);
            
            PlaceManager.Instance.InitTargetHeight();
        }
    }

    public void OnReturnedToPool()
    {

    }
}
