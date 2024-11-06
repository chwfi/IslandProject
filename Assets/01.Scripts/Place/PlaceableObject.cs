using DG.Tweening;
using UnityEngine;
using Util;

public class PlaceableObjectSaveData
{
    public Vector3 placedPosition;
    public PlaceableObjectState state;
} 

public enum PlaceableObjectState
{
    Inactive,
    Active,
    WaitForCompletion
}       

public abstract class PlaceableObject : MonoBehaviour, IPoolable
{
    [SerializeField] private PlaceableObjectData _objectData;

    private BuildOptionUI _buildOptionUI;
    private PlaceableChecker _placeableChecker;
    private GameObject _modelObject;

    public GameObject ModelObject => _modelObject;
    public PlaceableObjectData ObjectData => _objectData;
    public PlaceableObjectState ObjectState { get; private set; }

    private const string HARVEST_BUTTON_PREFAB = "HarvestButtonUI";
    private float _timer;

    public void SetPlaceableObject(PlaceableObjectData data) 
    {
        _placeableChecker = transform.GetComponentInChildren<PlaceableChecker>();   
        _modelObject = transform.Find("Object").gameObject; 

        _objectData = data;

        _buildOptionUI = transform.GetComponentInChildren<BuildOptionUI>();
        _buildOptionUI.SetObject(this);

        if (!_placeableChecker.gameObject.activeInHierarchy)
        {
            _placeableChecker.gameObject.SetActive(true);
        }
    }

    public void OnPlace()
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
        }, 
        () => 
        {
            PoolManager.Instance.Return(this);
        });
    }

    public void SetTimer(float endTime)
    {
        if (ObjectState == PlaceableObjectState.WaitForCompletion) 
            return;

        _timer += Time.deltaTime;
        if (_timer >= endTime)
        {
            CheckState();
        }
    }

    public abstract void CheckState();

    public virtual void OnTakenFromPool()
    {
        ModelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.01f);
        _placeableChecker.gameObject.SetActive(false);
        _buildOptionUI.gameObject.SetActive(false);
            
        PlaceManager.Instance.InitTargetHeight();
    }

    public void OnHarvest()
    {
        ObjectState = PlaceableObjectState.Inactive;
        _timer = 0;
    }
    
    protected void ShowHarvestUI()
    {
        var ui = PoolManager.Instance.Take(HARVEST_BUTTON_PREFAB, transform) as HarvestButtonUI;
        ui.SetObject(this, ObjectData.material);  
    }

    public PlaceableObjectSaveData ToSaveData()
    {
        return new PlaceableObjectSaveData
        {
            placedPosition = transform.position,
            state = ObjectState
        };
    }

    public void LoadFrom(PlaceableObjectSaveData saveData)
    {
        transform.position = saveData.placedPosition;
        ObjectState = saveData.state;


        if (saveData.state == PlaceableObjectState.WaitForCompletion)
            ShowHarvestUI();
    }

    public void OnReturnedToPool()
    {

    }
}
