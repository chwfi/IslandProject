using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scrollAmount;
    [SerializeField] private float _minScroll;
    [SerializeField] private float _maxScroll;

    private CinemachineVirtualCamera _camera;

    private Vector2 _lastTouchPosition;


    private void Awake() 
    {
        _camera = transform.Find("MainCam").GetComponent<CinemachineVirtualCamera>(); 
    }

    public void SetCameraFollow(Transform root)
    {
        _camera.Follow = root;
    }

    public void InitCameraFollow()
    {
        _camera.Follow = _cameraRoot;
    }

    #region Move & Rotate
    private void Update() 
    {
        #if UNITY_IOS
        MoveCamera_Mobile();
        ZoomCamera_Mobile();
        #endif

        #if UNITY_EDITOR
        MoveCamera();
        ZoomCamera();
        #endif
    }

    private void MoveCamera_Mobile()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _lastTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = touch.position - _lastTouchPosition;

                Vector3 moveDirection = new Vector3(deltaPosition.x, 0, deltaPosition.y);
                moveDirection = Quaternion.Euler(0, _cameraRoot.eulerAngles.y, 0) * moveDirection;

                _cameraRoot.position += _moveSpeed * Time.deltaTime * (- moveDirection);

                _lastTouchPosition = touch.position;
            }
        }
    }

    private void ZoomCamera_Mobile()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * _scrollAmount);
        }  
    }

    void Zoom(float increment)
    {
        var wheel = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        wheel.m_CameraDistance = Mathf.Clamp(wheel.m_CameraDistance - increment, _minScroll, _maxScroll);
    }


    private void MoveCamera()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = 40 * Time.deltaTime * new Vector3(x, 0, z).normalized;
        _cameraRoot.position += move;
    }

    private void ZoomCamera()
    {
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");
        var wheel = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        wheel.m_CameraDistance += -scroollWheel * Time.deltaTime * 250;
    }
    #endregion
}
