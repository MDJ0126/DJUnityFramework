using UnityEngine;

namespace Game
{
    public abstract class Movement : MonoBehaviour
    {
        #region Inspector

        public float maxSpeed = 12f;
        public float acceleration = 2048f;
        public float deceleration = 2048f;
        public float rotationSpeed = 720f;
        public float jumpVelocity = 10f;

        #endregion

        protected Rigidbody rigidbodyComp;
        protected CapsuleCollider capsule;
        private LayerMask groundLayer;

        public Vector3 MoveInput { get; set; } = Vector3.zero;
        public Vector3 Velocity { get; set; } = Vector3.zero;
        public Vector3 NormalizedVelocity => Velocity / maxSpeed;
        public bool IsGrounded { get; private set; } = false;
        public bool IsJumping { get; private set; } = false;
        public bool IsFalling { get; private set; } = false;

        protected virtual void Awake()
        {
            rigidbodyComp = GetComponentInChildren<Rigidbody>();
            capsule = GetComponentInChildren<CapsuleCollider>();
            groundLayer = LayerMask.GetMask("Ground");
        }

        protected virtual void FixedUpdate()
        {
            CheckGround();
            CheckJumpState();
            UpdateMovement();
            UpdateRotation();
        }

        /// <summary>
        /// 점프 상태 체크
        /// </summary>
        private void CheckJumpState()
        {
            if (IsJumping)
            {
                if (rigidbodyComp.linearVelocity.y <= 0f)
                {
                    IsJumping = false;
                    IsFalling = true;
                }
            }
            else if (IsFalling)
            {
                if (IsGrounded)
                {
                    IsFalling = false;
                }
            }
        }

        /// <summary>
        /// 지면에 있는지 체크 (Ray를 발에서 계속 쏜다)
        /// </summary>
        private void CheckGround()
        {
            Vector3 groundCheckPosition = new Vector3(
                capsule.bounds.center.x,
                capsule.bounds.min.y + 0.05f,
                capsule.bounds.center.z
            );

            IsGrounded = Physics.CheckSphere(
                groundCheckPosition,
                capsule.radius,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );
        }

        /// <summary>
        /// 이동 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            if (!IsGrounded || IsJumping)
                return;

            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.2f, groundLayer))
            {
                // 이동 입력 백터
                Vector3 inputDir = MoveInput.normalized;

                // 이동 입력 백터와 법선 백터를 이용한 경사로 백터
                Vector3 slopeMoveDir = Vector3.ProjectOnPlane(inputDir, hit.normal).normalized;

                // 경사로 백터를 반영한다.
                Vector3 targetVelocity = slopeMoveDir * maxSpeed * Mathf.Clamp01(MoveInput.magnitude);

                float accel = MoveInput.sqrMagnitude > 0f ? acceleration : deceleration;

                Velocity = Vector3.MoveTowards(
                    Velocity,
                    targetVelocity,
                    accel * Time.fixedDeltaTime
                );

                rigidbodyComp.linearVelocity = Velocity;
            }
        }

        /// <summary>
        /// 회전 업데이트
        /// </summary>
        private void UpdateRotation()
        {
            Vector3 direction = MoveInput;

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
            if (!IsGrounded) return;

            Vector3 velocity = rigidbodyComp.linearVelocity;
            velocity.y = jumpVelocity;

            rigidbodyComp.linearVelocity = velocity;

            IsJumping = true;
            IsFalling = false;
        }
    }
}
