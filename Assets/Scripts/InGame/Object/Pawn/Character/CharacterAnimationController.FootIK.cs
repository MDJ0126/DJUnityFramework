using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public partial class CharacterAnimationController
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

        /// <summary>
        /// 캐릭터가 정지한 동안 양발의 지면 위치와 몸 높이를 IK로 보정
        /// </summary>
        private void OnAnimatorIK(int layerIndex)
        {
            if (!_boneAnimator)
                return;

            // 이동·점프·낙하 중에는 원본 애니메이션을 유지하고 정지 상태에서만 IK를 적용한다.
            bool isIdle =
                _movement.IsGrounded &&
                !_movement.IsJumping &&
                !_movement.IsFalling &&
                _owner.Movement.NormalizedHorizontalVelocity < 0.001f;

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

        /// <summary>
        /// 발 아래 지면을 검사하여 목표 위치, 회전 및 지면 간격 계산
        /// </summary>
        private FootIKData CalculateFootIK(AvatarIKGoal foot)
        {
            FootIKData data = new FootIKData();

            data.footPosition = _boneAnimator.GetIKPosition(foot);
            data.footRotation = _boneAnimator.GetIKRotation(foot);

            // 애니메이션 발 위치 위에서 아래로 Ray를 쏴 단차가 있는 지면도 찾는다.
            Vector3 rayOrigin = data.footPosition + Vector3.up * footRayStartHeight;
            float rayDistance = footRayStartHeight + footRayDistance;

            Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.yellow);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, _groundLayer, QueryTriggerInteraction.Ignore))
                return data;

            data.isHit = true;

            float groundY = hit.point.y + footOffset;

            data.targetPosition = data.footPosition;
            data.targetPosition.y = groundY;

            // 현재 발의 위쪽 축을 지면 법선에 맞춰 경사면 방향으로 회전시킨다.
            data.targetRotation = Quaternion.FromToRotation(
                data.footRotation * Vector3.up,
                hit.normal
            ) * data.footRotation;

            data.groundGap = Mathf.Max(0f, data.footPosition.y - groundY);

            Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.green);

            return data;
        }

        /// <summary>
        /// 더 낮은 발의 간격만큼 몸을 내려 양발이 지면에 닿도록 보정
        /// </summary>
        private void UpdateBodyPosition(FootIKData leftFoot, FootIKData rightFoot)
        {
            float leftGap = leftFoot.isHit ? leftFoot.groundGap : 0f;
            float rightGap = rightFoot.isHit ? rightFoot.groundGap : 0f;

            // 양발 중 더 큰 높이 차이를 사용하되 최대 하강량을 넘지 않는다.
            float maxGap = Mathf.Max(leftGap, rightGap);
            float targetOffset = -Mathf.Min(maxGap, maxBodyDrop);

            // 몸이 순간적으로 튀지 않도록 현재 오프셋에서 목표값으로 보간한다.
            _currentBodyOffset = Mathf.Lerp(
                _currentBodyOffset,
                targetOffset,
                bodyAdjustSpeed * Time.deltaTime
            );

            Vector3 bodyPosition = _boneAnimator.bodyPosition;
            bodyPosition.y += _currentBodyOffset;
            _boneAnimator.bodyPosition = bodyPosition;
        }

        /// <summary>
        /// 지면을 찾은 발에만 계산된 IK 위치와 회전 적용
        /// </summary>
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

        /// <summary>
        /// 공통 활성 가중치에 위치·회전별 설정값을 곱해 Animator에 반영
        /// </summary>
        private void SetFootIKWeight(AvatarIKGoal foot, float weight)
        {
            _boneAnimator.SetIKPositionWeight(foot, weight * ikPositionWeight);
            _boneAnimator.SetIKRotationWeight(foot, weight * ikRotationWeight);
        }

        /// <summary>
        /// 이동 상태로 전환될 때 몸 보정과 양발 IK 가중치 초기화
        /// </summary>
        private void ResetIK()
        {
            _currentBodyOffset = 0f;

            SetFootIKWeight(AvatarIKGoal.LeftFoot, 0f);
            SetFootIKWeight(AvatarIKGoal.RightFoot, 0f);
        }
    }
}
