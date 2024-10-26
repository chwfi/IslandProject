using DG.Tweening;
using UnityEngine;

public class PlaceableObject : MonoBehaviour, IPoolable
{
    protected PlaceableObjectData _objectData;

    private BuildOptionUI _buildOptionUI;
    private PlaceableChecker _placeableChecker;
    private GameObject _modelObject;
    public GameObject ModelObject => _modelObject;

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
            _modelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.05f).OnComplete(() =>
            {
                var effect = PoolManager.Instance.Take("DustEffect", transform);
            });

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

    public void OnTakenFromPool()
    {

    }

    public void OnReturnedToPool()
    {

    }
}
