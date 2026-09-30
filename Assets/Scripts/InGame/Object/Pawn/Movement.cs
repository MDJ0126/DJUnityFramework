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
        public Vector3 Velocity { get; set; } = Vector3.zero;
        public float NormalizedVelocity
        {
            get
            {
                Vector3 horizontalVelocity = Velocity;
                horizontalVelocity.y = 0f;
                return horizontalVelocity.magnitude / maxSpeed;
            }
        }

        public bool IsGrounded { get; private set; } = false;
        public bool IsCanJump { get; private set; } = false;
        public bool IsJumping { get; private set; } = false;
        public bool IsFalling { get; private set; } = false;
        public bool IsSprint { get; set; } = false;

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
        /// 지면에 있는지 체크 (Ray를 발에서 계속 쏜다)
        /// </summary>
        private void CheckGround()
        {
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
        /// 이동 업데이트
        /// </summary>
        private void UpdateMovement()
        {
            if (!IsGrounded || IsJumping)
                return;

            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 1.2f, groundLayer))
            {
                Vector3 inputDir = MoveInput.normalized;

                Vector3 slopeMoveDir = Vector3.ProjectOnPlane(inputDir, hit.normal).normalized;

                float speed = IsSprint ? maxSpeed : walkMaxSpeed;

                Vector3 targetVelocity = slopeMoveDir * speed * Mathf.Clamp01(MoveInput.magnitude);

                float accel = MoveInput.sqrMagnitude > 0f ? acceleration : deceleration;

                // 현재 Rigidbody의 Y 속도는 건드리지 않는다.
                Vector3 currentVelocity = rigidbodyComp.linearVelocity;

                Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

                Vector3 targetHorizontalVelocity = new Vector3(targetVelocity.x, 0f, targetVelocity.z);

                Vector3 horizontalVelocity = Vector3.MoveTowards(
                    currentHorizontalVelocity,
                    targetHorizontalVelocity,
                    accel * Time.fixedDeltaTime
                );

                Velocity = horizontalVelocity;

                rigidbodyComp.linearVelocity = new Vector3(
                    horizontalVelocity.x,
                    currentVelocity.y,
                    horizontalVelocity.z
                );
            }
        }

        /// <summary>
        /// 회전 업데이트
        /// </summary>
        private void UpdateRotation()
        {
            // 물리 충돌로 생긴 회전 제거
            Vector3 angularVelocity = rigidbodyComp.angularVelocity;
            angularVelocity.y = 0f;
            rigidbodyComp.angularVelocity = angularVelocity;

            Vector3 direction;
            if (IsSprint)
            {
                direction = MoveInput;
            }
            else
            {
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
