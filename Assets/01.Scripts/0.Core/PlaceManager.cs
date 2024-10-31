using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlaceManager : MonoSingleton<PlaceManager>
{
    [Header("Database")]
    [SerializeField] private PlaceableDatabase _placeableDatabase;

    [Header("Grid Target")]
    [SerializeField] private GameObject _targetRoot;
    [SerializeField] private Transform _landInitTransform;
    [SerializeField] private Transform _waterInitTransform;

    [Header("PlaceableObject Setting")]
    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeMaterial;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;

    public PlaceableObject PreviousPlaceableObject { get; private set; }
    public PlaceableObject CurrentPlaceableObject { get; private set; }

    public Stack<PlaceableObject> LandObjects { get; private set; }
    public Stack<PlaceableObject> WaterObjects { get; private set; }

    private Camera _mainCamera;
    private Vector3 _gridPosition;
    private float _gridSize;
    private float _initialHeight;

    private bool _isDragging = false;

    public PlaceableDatabase PlaceableDatabase => _placeableDatabase;
    public Material PositiveMaterial => _positiveMaterial;
    public Material NegativeMaterial => _negativeMaterial;
    public float RayDistance => _rayDistance;
    public LayerMask LayerMask => _layerMask;

    private void Start() 
    {
        _mainCamera = Camera.main;   
        _initialHeight = _targetRoot.transform.position.y;
        LandObjects = new Stack<PlaceableObject>();
        WaterObjects = new Stack<PlaceableObject>();

        InitTargetHeight();
    }

    public void InitTargetHeight()
    {
        SetTargetPosition(_targetRoot.transform.position.x, _targetRoot.transform.position.z);
    }

    public void SetPlaceableObject(PlaceableObjectData data)
    {
        PopupUIManager.Instance.MovePopupUI("CreatePanel", new Vector3(0, -375, 0));

        SetPreviousObject();

        CurrentPlaceableObject = PoolManager.Instance.Take(data.objectName, null) as PlaceableObject;
        _gridSize = CurrentPlaceableObject.transform.localScale.x * 4f;

        SetObjectPosition(data);
        
        CurrentPlaceableObject.transform.position = _targetRoot.transform.position;
        CurrentPlaceableObject.SetPlaceableObject(data);
    }

    private void SetPreviousObject()
    {
        if (CurrentPlaceableObject == null)
            return;

        PreviousPlaceableObject = CurrentPlaceableObject;

        if (PreviousPlaceableObject.ObjectData.placeableObjectType == PlaceableObjectType.LandType)
            LandObjects.Push(PreviousPlaceableObject);
        else
            WaterObjects.Push(PreviousPlaceableObject);
    }

    public void SetObjectPosition(PlaceableObjectData data)
    {
        if (PreviousPlaceableObject == null)
            return;

        if (PreviousPlaceableObject.ObjectData.placeableObjectType != data.placeableObjectType)
        {
            if (data.placeableObjectType == PlaceableObjectType.WaterType)
            {
                if (WaterObjects.Count <= 0)
                {
                    SetTargetPosition(_waterInitTransform.position.x, _waterInitTransform.position.z);
                }
                else
                {
                    var value = WaterObjects.Pop();
                    SetTargetPosition(value.transform.position.x, value.transform.position.z);
                }
                return;
            }

            if (data.placeableObjectType == PlaceableObjectType.LandType)
            {
                if (LandObjects.Count <= 0)
                {
                    SetTargetPosition(_landInitTransform.position.x, _landInitTransform.position.z);
                }
                else
                {
                    var value = LandObjects.Pop();
                    SetTargetPosition(value.transform.position.x, value.transform.position.z);
                }
                return;
            }
        }
    }

    private void SetTargetPosition(float x, float z)
    {
        _targetRoot.transform.position = new Vector3(x, _initialHeight, z);
    }

    public void CancelPlace()
    {
        PoolManager.Instance.Return(CurrentPlaceableObject);
        InitTargetHeight();
    }

    private void Update() 
    {
        if (CurrentPlaceableObject == null) 
            return;

        HandleInput();
        if (_isDragging)
        {
            Follow();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == 
            CurrentPlaceableObject.ModelObject.transform)
            {
                _isDragging = true;
                CameraController.Instance.canControll = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            CameraController.Instance.canControll = true;
        }
    }

    private void Follow()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            _targetRoot.transform.position = targetPosition;
        }
    }

    private void LateUpdate() 
    {
        if (CurrentPlaceableObject == null) 
            return;

        _gridPosition.x = Mathf.Floor((_targetRoot.transform.position.x / _gridSize)) * _gridSize + _gridSize / 2f;    
        _gridPosition.z = Mathf.Floor((_targetRoot.transform.position.z / _gridSize)) * _gridSize + _gridSize / 2f;

        CurrentPlaceableObject.transform.position = 
        new Vector3(_gridPosition.x, CurrentPlaceableObject.transform.position.y, _gridPosition.z); 
    }
}
