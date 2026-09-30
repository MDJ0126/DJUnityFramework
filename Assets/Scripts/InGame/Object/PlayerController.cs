using UnityEngine;

namespace Game
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        #region Inspector

        [ReadOnly][SerializeField] private Pawn _possessTarget = null;

        [Header("Aim Settings")]
        [ReadOnly] public AimTarget aimTarget = null;
        public float aimDistance = 100f;
        public LayerMask aimMask;

        #endregion

        public bool IsPossessed { get; private set; } = false;
        private Pawn _lastPossessTarget = null;

        private Transform _mainCameraTransform;
        private Movement _movement;

        /// <summary>
        /// 입력 방향 계산에 사용할 메인 카메라 Transform 캐싱
        /// </summary>
        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
        }

        /// <summary>
        /// 빙의 중 이동, 달리기, 점프 및 액션 입력 처리
        /// </summary>
        private void Update()
        {
            if (!IsPossessed) return;

            if (_movement)
            {
                // 이동
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                // 카메라의 수평 전방·우측 축으로 입력을 월드 이동 방향으로 변환한다.
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

        /// <summary>
        /// 카메라 이동 후 화면 중앙을 기준으로 에임 타겟 갱신
        /// </summary>
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
                // 캐릭터 전방과 에임 방향의 내적을 0~1로 변환해 Aim Rig 가중치로 사용한다.
                Vector3 direction = (aimTarget.target.position - _possessTarget.Transform.position).normalized;
                float dot = Vector3.Dot(_possessTarget.Transform.forward, direction);
                float value01 = Mathf.InverseLerp(-1f, 1f, dot);
                aimTarget.AimRig.weight = value01;

                // 화면 중앙 Ray가 맞은 지점을 사용하고, 없으면 최대 조준 거리를 사용한다.
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
                // 대상이 생략되면 에디터 토글을 위해 마지막으로 빙의했던 Pawn을 재사용한다.
                pawn = _lastPossessTarget;
            }
            _possessTarget = pawn;

            if (_possessTarget != null)
            {
                _lastPossessTarget = pawn;
                _possessTarget.Possess(this);

                // Pawn의 이동 입력과 SpringArm 카메라를 컨트롤러에 연결한다.
                _movement = _possessTarget.GetComponent<Movement>();
                var playerCameraController = Camera.main.GetComponent<PlayerCameraController>();
                playerCameraController.Bind(pawn.GetComponentInChildren<SpringArm>());
                IsPossessed = true;
                // 마우스 회전 중 커서가 게임 화면 밖으로 나가지 않도록 잠근다.
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public virtual void Unpossess()
        {
            // Pawn별 해제 처리를 먼저 호출한 뒤 입력과 조준 참조를 정리한다.
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
