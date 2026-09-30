using System.Collections;
using UnityEngine;

namespace Game
{
    public abstract class Movement : MonoBehaviour
    {
        private const float CAN_JUMP_TIME = 0.2f;

        #region Inspector

        public float maxSpeed = 10f;
        public float walkMaxSpeed = 3f;
        public float acceleration = 100f;
        public float deceleration = 150f;
        public float rotationSpeed = 720f;
        public float jumpVelocity = 7f;

        #endregion

        protected Rigidbody rigidbodyComp;
        protected CapsuleCollider capsule;
        private LayerMask groundLayer;

        public Vector3 MoveInput { get; set; } = Vector3.zero;
        public Vector3 HorizontalVelocity { get; set; } = Vector3.zero;
        public float NormalizedHorizontalVelocity
        {
            get
            {
                Vector3 horizontalVelocity = HorizontalVelocity;
                horizontalVelocity.y = 0f;
                return horizontalVelocity.magnitude / maxSpeed;
            }
        }

        public bool IsGrounded { get; private set; } = false;
        public bool IsCanJump { get; private set; } = false;
        public bool IsJumping { get; private set; } = false;
        public bool IsFalling { get; private set; } = false;
        public bool IsSprint { get; set; } = false;
        public bool IsCanMove { get; private set; } = false;

        /// <summary>
        /// 이동 계산에 사용할 물리 컴포넌트와 지면 레이어 캐싱
        /// </summary>
        protected virtual void Awake()
        {
            rigidbodyComp = GetComponentInChildren<Rigidbody>();
            capsule = GetComponentInChildren<CapsuleCollider>();
            groundLayer = LayerMask.GetMask("Ground");
        }

        /// <summary>
        /// 물리 프레임마다 이동 가능 여부, 지면, 점프, 이동, 회전을 순서대로 갱신
        /// </summary>
        protected virtual void FixedUpdate()
        {
            CheckCanMove();
            CheckGround();
            CheckJumpState();
            UpdateMovement();
            UpdateRotation();
        }

        /// <summary>
        /// 이동 가능 체크
        /// </summary>
        private void CheckCanMove()
        {
            if (IsFalling && HorizontalVelocity.sqrMagnitude > 0f)
            {
                IsCanMove = false;
            }

            if (!IsCanMove && rigidbodyComp.linearVelocity.sqrMagnitude <= 1f)
            {
                IsCanMove = true;
            }
        }

        /// <summary>
        /// 지면에 있는지 체크 (Ray를 발에서 계속 쏜다)
        /// </summary>
        private void CheckGround()
        {
            // 캡슐 바닥보다 살짝 위를 중심으로 검사해 작은 접촉 오차를 허용한다.
            Vector3 groundCheckPosition = new Vector3(
                capsule.bounds.center.x,
                capsule.bounds.min.y + 0.05f,
                capsule.bounds.center.z
            );

            bool isGrounded = Physics.CheckSphere(
                groundCheckPosition,
                capsule.radius,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );

            if (IsGrounded != isGrounded)
            {
                IsGrounded = isGrounded;
                if (!isGrounded)
                {
                    if (!IsJumping)
                    {
                        // 발판을 벗어난 직후에도 잠깐 점프를 허용하는 코요테 타임이다.
                        IsCanJump = true;
                        StartCoroutine(DelayCantJump());
                    }
                }
            }

            IEnumerator DelayCantJump()
            {
                yield return YieldInstructionCache.WaitForSeconds(CAN_JUMP_TIME);
                IsCanJump = false;
            }
        }

        /// <summary>
        /// 점프 상태 체크
        /// </summary>
        private void CheckJumpState()
        {
            // 상승 속도가 거의 사라지면 점프 상태를 낙하 상태로 전환한다.
            //if (IsJumping)
            {
                if (rigidbodyComp.linearVelocity.y < 0.2f)
                {
                    IsJumping = false;
                    IsFalling = true;
                }
            }
            if (IsFalling)
            {
                if (IsGrounded)
                {
                    IsFalling = false;
                }
            }
        }

        /// <summary>
        /// 이동 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            if (!IsGrounded || IsJumping || !IsCanMove) return;

            Vector3 inputDir = MoveInput.normalized;
            Vector3 moveDir = inputDir;
            bool isSlope = false;

            // 현재 지면의 법선을 구해 경사면을 따라 움직일 방향을 만든다.
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 1.2f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                isSlope = slopeAngle > 1f;

                moveDir = Vector3.ProjectOnPlane(inputDir, hit.normal).normalized;
                Debug.DrawRay(hit.point, hit.normal, Color.red);
            }

            // 달리기 여부와 입력 크기를 반영한 목표 수평 속도를 계산한다.
            float speed = IsSprint ? maxSpeed : walkMaxSpeed;
            Vector3 targetVelocity = moveDir * speed * Mathf.Clamp01(MoveInput.magnitude);
            float accel = MoveInput.sqrMagnitude > 0f ? acceleration : deceleration;

            Vector3 currentVelocity = rigidbodyComp.linearVelocity;
            Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            Vector3 targetHorizontalVelocity = new Vector3(targetVelocity.x, 0f, targetVelocity.z);
            Vector3 horizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetHorizontalVelocity, accel * Time.fixedDeltaTime);

            HorizontalVelocity = horizontalVelocity;

            // 수평 이동만 보간하고 중력으로 계산된 수직 속도는 보존한다.
            float verticalVelocity = currentVelocity.y;

            if (!isSlope && verticalVelocity > 0f) verticalVelocity = 0f;

            rigidbodyComp.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        }

        /// <summary>
        /// 회전 업데이트
        /// </summary>
        private void UpdateRotation()
        {
            if (!IsCanMove || MoveInput.sqrMagnitude == 0f) return;

            // 물리 충돌로 생긴 회전 제거
            Vector3 angularVelocity = rigidbodyComp.angularVelocity;
            angularVelocity.y = 0f;
            rigidbodyComp.angularVelocity = angularVelocity;

            Vector3 direction;
            if (IsSprint)
            {
                // 달릴 때는 입력 방향을 바라본다.
                direction = MoveInput;
            }
            else
            {
                // 걸을 때는 카메라가 바라보는 수평 방향을 유지한다.
                direction = Camera.main.transform.forward;
            }
            // Yaw 축만 사용하기
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            Quaternion nextRotation = Quaternion.RotateTowards(rigidbodyComp.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rigidbodyComp.MoveRotation(nextRotation);
        }

        /// <summary>
        /// 점프
        /// </summary>
        public void Jump()
        {
            if (!IsGrounded && !IsCanJump) return;

            // 기존 수평 속도를 유지하면서 수직 속도만 점프 값으로 교체한다.
            Vector3 velocity = rigidbodyComp.linearVelocity;
            velocity.y = jumpVelocity;

            rigidbodyComp.linearVelocity = velocity;

            IsJumping = true;
            IsFalling = false;

            if (!IsGrounded && IsCanJump)
            {
                IsCanJump = false;
            }
        }
    }
}
