using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private float footRayStartHeight = 0.5f;
        [SerializeField] private float footRayDistance = 1.0f;
        [SerializeField] private float footOffset = 0.05f;
        [SerializeField] private float maxFootHeight = 0.15f;

        [SerializeField, Range(0f, 1f)] private float ikPositionWeight = 0.35f;
        [SerializeField, Range(0f, 1f)] private float ikRotationWeight = 0.8f;
        [SerializeField] private float ikBlendSpeed = 10f;

        #endregion

        private Character _owner;
        private Animator _boneAnimator;
        private Movement _movement;
        private LayerMask _groundLayer;

        private float _leftFootIKWeight;
        private float _rightFootIKWeight;

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
            if (!_boneAnimator) return;

            UpdateFootIK(AvatarIKGoal.LeftFoot);
            UpdateFootIK(AvatarIKGoal.RightFoot);
        }

        private void UpdateFootIK(AvatarIKGoal foot)
        {
            Vector3 footPosition = _boneAnimator.GetIKPosition(foot);
            Quaternion footRotation = _boneAnimator.GetIKRotation(foot);

            Vector3 rayOrigin = footPosition + Vector3.up * footRayStartHeight;
            float rayDistance = footRayStartHeight + footRayDistance;

            //Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.yellow);

            bool isHit = Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                rayDistance,
                _groundLayer,
                QueryTriggerInteraction.Ignore
            );

            if (!isHit)
            {
                float weight = UpdateIKWeight(foot, false);
                ApplyIKWeight(foot, weight);
                return;
            }

            float groundY = hit.point.y + footOffset;
            float heightDifference = footPosition.y - groundY;

            bool shouldApplyIK = heightDifference <= maxFootHeight;
            float ikWeight = UpdateIKWeight(foot, shouldApplyIK);

            ApplyIKWeight(foot, ikWeight);

            if (!shouldApplyIK)
                return;

            Vector3 targetPosition = footPosition;
            targetPosition.y = groundY;

            Quaternion targetRotation = Quaternion.FromToRotation(
                footRotation * Vector3.up,
                hit.normal
            ) * footRotation;

            _boneAnimator.SetIKPosition(foot, targetPosition);
            _boneAnimator.SetIKRotation(foot, targetRotation);

            Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.green);
        }

        private float UpdateIKWeight(AvatarIKGoal foot, bool active)
        {
            float targetWeight = active ? 1f : 0f;

            if (foot == AvatarIKGoal.LeftFoot)
            {
                _leftFootIKWeight = Mathf.MoveTowards(
                    _leftFootIKWeight,
                    targetWeight,
                    ikBlendSpeed * Time.deltaTime
                );

                return _leftFootIKWeight;
            }

            _rightFootIKWeight = Mathf.MoveTowards(
                _rightFootIKWeight,
                targetWeight,
                ikBlendSpeed * Time.deltaTime
            );

            return _rightFootIKWeight;
        }

        private void ApplyIKWeight(AvatarIKGoal foot, float weight)
        {
            _boneAnimator.SetIKPositionWeight(foot, weight * ikPositionWeight);
            _boneAnimator.SetIKRotationWeight(foot, weight * ikRotationWeight);
        }
    }
}