using UnityEngine;

namespace Game
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        #region Inspector

        [ReadOnly] [SerializeField] private Pawn _possessTarget = null;

        [Header("Aim Settings")]
        [ReadOnly][SerializeField] private Transform _aimTarget = null;
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
            if (_aimTarget)
            {
                if (_possessTarget.Movement.IsSprint)
                {
                    _possessTarget.spineAim.weight = 0f;
                }
                else
                {
                    _possessTarget.spineAim.weight = 1f;
                }

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

                if (Physics.Raycast(ray, out var hit, aimDistance, aimMask))
                {
                    _aimTarget.position = hit.point;
                }
                else
                {
                    _aimTarget.position = ray.GetPoint(aimDistance);
                }
            }
        }

        /// <summary>
        /// 빙의
        /// </summary>
        public void Possess(Pawn pawn = null)
        {
            if (pawn == null)
            {
                pawn = _lastPossessTarget;
            }

            _possessTarget = pawn;
            _lastPossessTarget = pawn;
            _aimTarget = pawn.aimTarget;
            _movement = _possessTarget.GetComponent<Movement>();
            var playerCameraController = Camera.main.GetComponent<PlayerCameraController>();
            playerCameraController.Bind(pawn.GetComponentInChildren<SpringArm>());
            IsPossessed = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public void Unpossess()
        {
            _possessTarget.ResetAimTargetPosition();
            _aimTarget = null;
            _movement = null;
            IsPossessed = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _possessTarget = null;
        }
    }
}