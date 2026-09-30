using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        #region Inspector

        [ReadOnly] [SerializeField] private Pawn _possessTarget = null;

        [Header("Aim Settings")]
        [ReadOnly] public AimTarget aimTarget = null;
        public float aimDistance = 100f;
        public LayerMask aimMask;

        #endregion

        public bool IsPossessed { get; private set; } = false;
        private Pawn _lastPossessTarget = null;

        private Transform _mainCameraTransform;
        private Movement _movement;

        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (!IsPossessed) return;

            if (_movement)
            {
                // 이동
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 cameraForward = Vector3.ProjectOnPlane(_mainCameraTransform.forward, Vector3.up).normalized;
                Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
                Vector2 input = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
                _movement.MoveInput = cameraForward * input.y + cameraRight * input.x;
                _movement.IsSprint = Input.GetKey(KeyCode.LeftShift);

                // 점프
                bool isJump = Input.GetKeyUp(KeyCode.Space);
                if (isJump)
                {
                    _movement.Jump();
                }

                // 좌클릭
                if (Input.GetMouseButton(0))
                {
                    _possessTarget.AnimationController.PlayActionTest();
                }


                // 우클릭
            }
        }

        private void LateUpdate()
        {
            UpdateAimTarget();
        }

        /// <summary>
        /// 에임 타겟 업데이트
        /// </summary>
        private void UpdateAimTarget()
        {
            if (_possessTarget != null && aimTarget != null)
            {
                Vector3 direction = (aimTarget.target.position - _possessTarget.Transform.position).normalized;
                float dot = Vector3.Dot(_possessTarget.Transform.forward, direction);
                float value01 = Mathf.InverseLerp(-1f, 1f, dot);
                aimTarget.AimRig.weight = value01;

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out var hit, aimDistance, aimMask))
                {
                    aimTarget.target.position = hit.point;
                }
                else
                {
                    aimTarget.target.position = ray.GetPoint(aimDistance);
                }
            }
        }

        /// <summary>
        /// 빙의
        /// </summary>
        public virtual void Possess(Pawn pawn = null)
        {
            if (pawn == null)
            {
                pawn = _lastPossessTarget;
            }
            _possessTarget = pawn;

            if (_possessTarget != null)
            {
                _lastPossessTarget = pawn;
                _possessTarget.Possess(this);

                _movement = _possessTarget.GetComponent<Movement>();
                var playerCameraController = Camera.main.GetComponent<PlayerCameraController>();
                playerCameraController.Bind(pawn.GetComponentInChildren<SpringArm>());
                IsPossessed = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public virtual void Unpossess()
        {
            _possessTarget.Unpossess(this);
            aimTarget = null;
            _movement = null;
            IsPossessed = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _possessTarget = null;
        }
    }
}