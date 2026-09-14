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
        private float _prevY = 0f;

        protected virtual void Awake()
        {
            rigidbodyComp = GetComponentInChildren<Rigidbody>();
            capsule = GetComponentInChildren<CapsuleCollider>();
            groundLayer = LayerMask.NameToLayer("Ground");
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
            if (_prevY < rigidbodyComp.linearVelocity.y && rigidbodyComp.linearVelocity.y > 0f)
            {
                IsJumping = true;
            }
            else if (IsJumping)
            {
                if (rigidbodyComp.linearVelocity.y <= 0f)
                {
                    IsFalling = true;
                    IsJumping = false;
                }
            }
            else if (IsGrounded)
            {
                IsFalling = false;
            }

            _prevY = rigidbodyComp.linearVelocity.y;
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
                1 << groundLayer,
                QueryTriggerInteraction.Ignore
            );
        }

        /// <summary>
        /// 이동 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            if (!IsGrounded) return;

            Vector3 targetVelocity = MoveInput * maxSpeed;
            float accel = MoveInput.sqrMagnitude > 0f ? acceleration : deceleration;
            Velocity = Vector3.MoveTowards(Velocity, targetVelocity, accel * Time.fixedDeltaTime);
            rigidbodyComp.linearVelocity = new Vector3(Velocity.x, rigidbodyComp.linearVelocity.y, Velocity.z);
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
            rigidbodyComp.linearVelocity = new Vector3(rigidbodyComp.linearVelocity.x, jumpVelocity, rigidbodyComp.linearVelocity.z);
        }
    }
}
