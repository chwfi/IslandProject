using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scrollAmount;
    [SerializeField] private float _minScroll;
    [SerializeField] private float _maxScroll;

    private CinemachineVirtualCamera _camera;

    private void Awake() 
    {
        _camera = transform.Find("MainCam").GetComponent<CinemachineVirtualCamera>();    
    }

    private void Update() 
    {
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = _moveSpeed * Time.deltaTime * new Vector3(x, 0, z).normalized;
        _cameraRoot.position += move;
    }

    private void ZoomCamera()
    {
        float scroollWheel = Input.GetAxis("Mouse ScrollWheel");
        var wheel = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        wheel.m_CameraDistance += -Mathf.Clamp(scroollWheel, _minScroll, _maxScroll) * Time.deltaTime * _scrollAmount;
    }
}
