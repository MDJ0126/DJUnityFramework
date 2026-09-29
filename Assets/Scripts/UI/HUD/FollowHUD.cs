using UnityEngine;

public abstract class FollowHUD : MonoBehaviour
{
    #region Inspector

    public bool followCameraSize = true;

    #endregion

    private Transform _transform = null;
    public Transform MyTransform
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

    protected virtual void OnEnable()
    {

    }

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
        this.MyTransform.position = targetPos + _offset;

        if (!target.gameObject.activeSelf) Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}