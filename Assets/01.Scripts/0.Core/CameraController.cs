using Cinemachine;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scrollAmount;
    [SerializeField] private float _minScroll;
    [SerializeField] private float _maxScroll;

    [SerializeField] private CinemachineVirtualCamera _camera;
    public CinemachineVirtualCamera Cam => _camera;
    public Transform CameraRoot => _cameraRoot;
    
    private Vector2 _lastTouchPosition;

    [HideInInspector] public bool canControll;
    [HideInInspector] public bool isMoving;

    private void Awake() 
    {
        canControll = true;
    }

    #region Move & Rotate
    private void Update() 
    {
        #if UNITY_IOS
        if (canControll)
        {
            MoveCamera_Mobile();
            ZoomCamera_Mobile();
        }
        #endif

        #if UNITY_EDITOR
        MoveCamera();
        ZoomCamera();
        #endif
    }

    private void MoveCamera_Mobile()
    {
        if (Input.touchCount != 1) return;

        Touch touch = Input.GetTouch(0);

        Ray ray = GameManager.Instance.MainCam.ScreenPointToRay(touch.position);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Land")
        || hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (touch.phase == TouchPhase.Began)
            {
                _lastTouchPosition = touch.position;
                isMoving = true; 
                return; 
            }

            if (touch.phase == TouchPhase.Moved && isMoving)
            {
                Vector2 deltaPosition = touch.position - _lastTouchPosition;

                Vector3 moveDirection = new Vector3(deltaPosition.x, 0, deltaPosition.y);
                moveDirection = Quaternion.Euler(0, _cameraRoot.eulerAngles.y, 0) * moveDirection;

                _cameraRoot.position += _moveSpeed * Time.deltaTime * (-moveDirection);

                _lastTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isMoving = false;
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

        // 카메라의 forward와 right 벡터에 따라 방향을 계산
        Vector3 direction = (_cameraRoot.forward * z + _cameraRoot.right * x).normalized;

        // Y축 움직임을 방지 (카메라가 위아래로 움직이지 않도록)
        direction.y = 0;

        // 움직임 벡터 계산 및 적용
        Vector3 move = 40 * Time.deltaTime * direction;
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
