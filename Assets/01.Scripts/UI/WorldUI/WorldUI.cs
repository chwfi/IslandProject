using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldUI : PoolableMono
{
    [SerializeField] private float _scaleFactor;

    private Canvas _canvas;

    private void Start() 
    {
        _canvas = transform.GetComponent<Canvas>();
        _canvas.worldCamera = GameManager.Instance.MainCam;
        _canvas.planeDistance = 50;
    }

    private void Update()
    {
        Vector3 cameraRotation = GameManager.Instance.MainCam.transform.rotation * Vector3.forward;
        Vector3 posTarget = transform.position + cameraRotation;
        transform.LookAt(posTarget);

        float distance = Vector3.Distance(GameManager.Instance.MainCam.transform.position, transform.position);
        float fov = GameManager.Instance.MainCam.fieldOfView;
        
        float scale = _scaleFactor * distance / fov;
        transform.localScale = Vector3.one * scale;
    }
}
