using UnityEngine;

public abstract class FollowHUD : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject root;
    [SerializeField] private bool _isFollowCameraSize = true;
    [SerializeField] private float shrinkStartDistance = 5f;
    [SerializeField] private float hideDistance = 20f;

    #endregion

    private Transform _transform = null;
    public Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = this.transform;
            }
            return _transform;
        }
    }

    [HideInInspector] public Transform target;
    private Camera _targetCamera = null;

    private Vector3 _offset = Vector3.zero;

    /// <summary>
    /// HUD가 추적할 월드 타겟과 화면 오프셋 설정
    /// </summary>
    public void SetTarget(Transform target, Vector2 offset = default)
    {
        if (target != null)
        {
            this.target = target;
            _offset = new Vector3(offset.x, offset.y);
            _targetCamera = Utils.GetMyCamera(target.gameObject);
            LateUpdate();
        }
    }

    /// <summary>
    /// HUD 활성화 시 파생 클래스에서 사용할 초기화 지점
    /// </summary>
    protected virtual void OnEnable()
    {

    }

    /// <summary>
    /// HUD가 풀로 돌아갈 때 이전 타겟과 카메라 참조 해제
    /// </summary>
    protected virtual void OnDisable()
    {
        this.target = null;
        _targetCamera = null;
    }

    /// <summary>
    /// 타겟 추적 갱신
    /// </summary>
    protected virtual void LateUpdate()
    {
        if (target == null || _targetCamera == null) return;

        // 타겟의 스크린 좌표를 가져온다.
        Vector3 targetPos = _targetCamera.WorldToScreenPoint(target.position);

        // UI의 위치를 변경해준다.
        Transform.position = targetPos + _offset;

        // HUD 사이즈 업데이트
        UpdateHUDSize(targetPos.z);

        if (!target.gameObject.activeSelf) Hide();
    }

    /// <summary>
    /// HUD 사이즈 업데이트
    /// </summary>
    private void UpdateHUDSize(float depth)
    {
        // 카메라에서 보이지 않는 오브젝트인 경우에는 그리지 않는다.
        bool isBlocked = Physics.Linecast(_targetCamera.transform.position, target.position, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
        if (isBlocked)
        {
            Transform.localScale = Vector3.zero;
            return;
        }

        // 원근감 표현 안 하는 경우
        if (!_isFollowCameraSize)
        {
            Transform.localScale = Vector3.one;
            return;
        }

        float distance = Vector3.Distance(target.position, _targetCamera.transform.position);

        // 숨김 거리는 월드 거리로 판정하고, 축소는 원근 투영의 깊이 비율을 따른다.
        // GameObject를 비활성화하면 추적이 중단되므로 크기만 0으로 만든다.
        if (depth <= 0f || distance >= hideDistance)
        {
            Transform.localScale = Vector3.zero;
            return;
        }

        float scale = _targetCamera.orthographic ? 1f : Mathf.Max(0.001f, shrinkStartDistance) / depth;
        Transform.localScale = Vector3.one * scale;
    }

    /// <summary>
    /// HUD 표시
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// HUD 숨기기
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
