using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour
    {
        #region Inspector

        [Header("Foot IK")]
        [SerializeField] private float footRayStartHeight = 0.5f;
        [SerializeField] private float footRayDistance = 1.0f;
        [SerializeField] private float footOffset = 0.13f;
        [SerializeField] private float ikPositionWeight = 1.0f;
        [SerializeField] private float ikRotationWeight = 0.8f;

        [Header("Body Offset")]
        [SerializeField] private float maxBodyDrop = 0.25f;
        [SerializeField] private float bodyAdjustSpeed = 8f;

        #endregion

        private Character _owner;
        private Animator _boneAnimator;
        private Movement _movement;
        private LayerMask _groundLayer;

        private float _currentBodyOffset;

        private struct FootIKData
        {
            public bool isHit;
            public Vector3 footPosition;
            public Quaternion footRotation;
            public Vector3 targetPosition;
            public Quaternion targetRotation;
            public float groundGap;
        }

        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _movement = _owner.GetComponent<Movement>();
            _boneAnimator = GetComponent<Animator>();
            _groundLayer = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            _boneAnimator.SetFloat("Velocity", _owner.Movement.NormalizedVelocity.magnitude);
            _boneAnimator.SetBool("IsJumping", _movement.IsJumping);
            _boneAnimator.SetBool("IsFalling", _movement.IsFalling);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!_boneAnimator)
                return;

            bool isIdle =
                _movement.IsGrounded &&
                !_movement.IsJumping &&
                !_movement.IsFalling &&
                _owner.Movement.NormalizedVelocity.sqrMagnitude < 0.001f;

            if (!isIdle)
            {
                ResetIK();
                return;
            }

            FootIKData leftFoot = CalculateFootIK(AvatarIKGoal.LeftFoot);
            FootIKData rightFoot = CalculateFootIK(AvatarIKGoal.RightFoot);

            UpdateBodyPosition(leftFoot, rightFoot);

            ApplyFootIK(AvatarIKGoal.LeftFoot, leftFoot);
            ApplyFootIK(AvatarIKGoal.RightFoot, rightFoot);
        }

        private FootIKData CalculateFootIK(AvatarIKGoal foot)
        {
            FootIKData data = new FootIKData();

            data.footPosition = _boneAnimator.GetIKPosition(foot);
            data.footRotation = _boneAnimator.GetIKRotation(foot);

            Vector3 rayOrigin = data.footPosition + Vector3.up * footRayStartHeight;
            float rayDistance = footRayStartHeight + footRayDistance;

            Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.yellow);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, _groundLayer, QueryTriggerInteraction.Ignore))
                return data;

            data.isHit = true;

            float groundY = hit.point.y + footOffset;

            data.targetPosition = data.footPosition;
            data.targetPosition.y = groundY;

            data.targetRotation = Quaternion.FromToRotation(
                data.footRotation * Vector3.up,
                hit.normal
            ) * data.footRotation;

            data.groundGap = Mathf.Max(0f, data.footPosition.y - groundY);

            Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.green);

            return data;
        }

        private void UpdateBodyPosition(FootIKData leftFoot, FootIKData rightFoot)
        {
            float leftGap = leftFoot.isHit ? leftFoot.groundGap : 0f;
            float rightGap = rightFoot.isHit ? rightFoot.groundGap : 0f;

            float maxGap = Mathf.Max(leftGap, rightGap);
            float targetOffset = -Mathf.Min(maxGap, maxBodyDrop);

            _currentBodyOffset = Mathf.Lerp(
                _currentBodyOffset,
                targetOffset,
                bodyAdjustSpeed * Time.deltaTime
            );

            Vector3 bodyPosition = _boneAnimator.bodyPosition;
            bodyPosition.y += _currentBodyOffset;
            _boneAnimator.bodyPosition = bodyPosition;
        }

        private void ApplyFootIK(AvatarIKGoal foot, FootIKData data)
        {
            if (!data.isHit)
            {
                SetFootIKWeight(foot, 0f);
                return;
            }

            SetFootIKWeight(foot, 1f);

            _boneAnimator.SetIKPosition(foot, data.targetPosition);
            _boneAnimator.SetIKRotation(foot, data.targetRotation);
        }

        private void SetFootIKWeight(AvatarIKGoal foot, float weight)
        {
            _boneAnimator.SetIKPositionWeight(foot, weight * ikPositionWeight);
            _boneAnimator.SetIKRotationWeight(foot, weight * ikRotationWeight);
        }

        private void ResetIK()
        {
            _currentBodyOffset = 0f;

            SetFootIKWeight(AvatarIKGoal.LeftFoot, 0f);
            SetFootIKWeight(AvatarIKGoal.RightFoot, 0f);
        }
    }
}