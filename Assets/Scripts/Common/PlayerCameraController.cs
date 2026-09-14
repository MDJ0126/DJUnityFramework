using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Mouse")]
    [SerializeField]
    private float mouseSensitivity = 3f;

    [SerializeField]
    private float minPitch = -70f;

    [SerializeField]
    private float maxPitch = 70f;

    [Header("Collision")]
    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private float cameraRadius = 0.2f;

    [SerializeField]
    private float collisionOffset = 0.05f;

    [SerializeField]
    private float minDistance = 0.2f;

    [SerializeField]
    private float returnSmoothTime = 0.1f;

    private SpringArm _springArm;

    // 빙의한 순간의 SpringArm 기준 카메라 월드 Offset
    private Vector3 _initialOffset;

    // 빙의한 순간의 카메라 월드 Rotation
    private Quaternion _initialRotation;

    // 마우스로 추가되는 회전
    private float _yaw;
    private float _pitch;

    // 충돌 처리용 거리
    private float _currentDistance;
    private float _distanceVelocity;

    /// <summary>
    /// 빙의할 캐릭터의 SpringArm을 카메라에 연결
    /// </summary>
    public void Bind(SpringArm springArm)
    {
        if (springArm == null)
            return;

        _springArm = springArm;

        // 에디터에서 잡아놓은 위치/회전으로 즉시 이동
        _springArm.ApplyCameraSetting(transform);

        // SpringArm Pivot -> Camera
        // 빙의 순간의 월드 Offset을 저장
        _initialOffset = _springArm.CameraWorldPosition - _springArm.transform.position;

        // 캐릭터 회전은 빙의 순간에만 설정
        _initialRotation = _springArm.CameraWorldRotation;

        _yaw = 0f;
        _pitch = 0f;

        _currentDistance = _initialOffset.magnitude;
        _distanceVelocity = 0f;
    }

    public void Unbind()
    {
        _springArm = null;
    }

    private void Update()
    {
        if (_springArm == null)
            return;

        UpdateRotation();
    }

    private void LateUpdate()
    {
        if (_springArm == null)
            return;

        UpdateCamera();
    }

    private void UpdateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * mouseSensitivity;
        _pitch += mouseY * mouseSensitivity;

        _pitch = Mathf.Clamp(
            _pitch,
            minPitch,
            maxPitch
        );
    }

    private void UpdateCamera()
    {
        Quaternion mouseRotation = Quaternion.Euler(_pitch, _yaw, 0f);

        // 에디터에서 잡은 초기 Offset 자체를
        // 마우스 회전만큼 Pivot 주위로 회전
        Vector3 desiredOffset = mouseRotation * _initialOffset;

        float desiredDistance = desiredOffset.magnitude;

        if (desiredDistance <= Mathf.Epsilon)
        {
            transform.SetPositionAndRotation(_springArm.transform.position, mouseRotation * _initialRotation);
            return;
        }

        Vector3 direction = desiredOffset / desiredDistance;

        Vector3 pivotPosition = _springArm.transform.position;

        float targetDistance = GetTargetDistance(
            pivotPosition,
            direction,
            desiredDistance
        );

        UpdateCameraDistance(targetDistance);

        Vector3 cameraPosition = pivotPosition + direction * _currentDistance;
        Quaternion cameraRotation = mouseRotation * _initialRotation;

        transform.SetPositionAndRotation(
            cameraPosition,
            cameraRotation
        );
    }

    /// <summary>
    /// Pivot에서 원래 카메라 위치까지 SphereCast하여 실제 사용할 거리를 계산
    /// </summary>
    private float GetTargetDistance(Vector3 pivotPosition, Vector3 direction, float desiredDistance)
    {
        direction.Normalize();

        // 1. 우선 중앙 Ray로 장애물 확인
        if (!Physics.Raycast(
                pivotPosition,
                direction,
                out RaycastHit rayHit,
                desiredDistance,
                collisionMask,
                QueryTriggerInteraction.Ignore))
        {
            // 장애물이 없으면 원래 카메라 거리 사용
            return desiredDistance;
        }

        // 2. 장애물이 있으면 카메라 반경을 고려해서 SphereCast
        if (Physics.SphereCast(
                pivotPosition,
                cameraRadius,
                direction,
                out RaycastHit sphereHit,
                rayHit.distance,
                collisionMask,
                QueryTriggerInteraction.Ignore))
        {
            return Mathf.Max(
                sphereHit.distance - collisionOffset,
                minDistance
            );
        }

        // SphereCast가 못 잡더라도
        // Ray는 이미 벽을 잡았으므로 최소한 Ray 위치까지만 이동
        return Mathf.Max(
            rayHit.distance - collisionOffset,
            minDistance
        );
    }

    /// <summary>
    /// 벽에 가까워질 때는 즉시 당기고, 거리가 다시 늘어날 때는 천천히 복귀
    /// </summary>
    private void UpdateCameraDistance(float targetDistance)
    {
        if (targetDistance < _currentDistance)
        {
            // 충돌할 때는 즉시 당김
            _currentDistance = targetDistance;
            _distanceVelocity = 0f;
            return;
        }

        // 장애물에서 빠져나오면 천천히 원래 위치로 복귀
        _currentDistance = Mathf.SmoothDamp(
            _currentDistance,
            targetDistance,
            ref _distanceVelocity,
            returnSmoothTime
        );

        // SmoothDamp가 목표 거리를 넘어가지 않도록 제한
        _currentDistance = Mathf.Min(_currentDistance, targetDistance);
    }
}