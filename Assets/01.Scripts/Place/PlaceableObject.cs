using DG.Tweening;
using UnityEngine;
using Util;

public class PlaceableObjectSaveData
{
    public Vector3 placedPosition;
    public PlaceableObjectState state;
    public float timer;
    public bool isPlaced;
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
    private bool _isPlaced = false;

    public void SetPlaceableObject() 
    {
        _placeableChecker = transform.GetComponentInChildren<PlaceableChecker>();   
        _modelObject = transform.Find("Object").gameObject; 
        _buildOptionUI = transform.GetComponentInChildren<BuildOptionUI>();
        _buildOptionUI.SetObject(this);

        if (!_placeableChecker.gameObject.activeInHierarchy)
        {
            _placeableChecker.gameObject.SetActive(true);
        }

        OnInactive();
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
            ObjectStateManager.Instance.ObjectList.Add(this);

            _isPlaced = true;
        }, 
        () => 
        {
            PoolManager.Instance.Return(this);
        });
    }

    public void CheckState()
    {
        switch (ObjectState)
        {
            case PlaceableObjectState.Inactive:
                ObjectState = PlaceableObjectState.Active;
                OnActive();
                break;
            case PlaceableObjectState.Active:
                ObjectState = PlaceableObjectState.WaitForCompletion;
                OnWaitForCompletion();
                break;
        }
    }

    public abstract void OnInactive();
    public abstract void OnActive();
    public abstract void OnWaitForCompletion();

    public void SetTimer(float endTime)
    {
        if (ObjectState == PlaceableObjectState.WaitForCompletion)
            return;

        _timer += Time.deltaTime;
        if (_timer >= endTime)
        {
            CheckState();
            _timer = 0;
        }
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
            state = ObjectState,
            timer = _timer,
            isPlaced = _isPlaced
        };
    }

    public void LoadFrom(PlaceableObjectSaveData saveData)
    {
        transform.position = saveData.placedPosition;
        ObjectState = saveData.state;
        _timer = saveData.timer;
        _isPlaced = saveData.isPlaced;

        CheckCondition(saveData);
    }

    private void CheckCondition(PlaceableObjectSaveData saveData)
    {
        if (saveData.state == PlaceableObjectState.WaitForCompletion)
            ShowHarvestUI();

        if (_isPlaced)
        {
            _modelObject.transform.DOMoveY(_placeableChecker.transform.position.y, 0.01f);
            _buildOptionUI.gameObject.SetActive(false);
            _placeableChecker.gameObject.SetActive(false);
        }
    }

    public void OnTakenFromPool()
    {
        SetPlaceableObject();
    }

    public void OnReturnedToPool()
    {
        ObjectStateManager.Instance.ObjectList.Remove(this);
    }
}
