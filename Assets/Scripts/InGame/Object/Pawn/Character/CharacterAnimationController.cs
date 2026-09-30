using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public partial class CharacterAnimationController : AnimationController
    {
        private Character _owner;
        private Animator _boneAnimator;
        private Movement _movement;
        private LayerMask _groundLayer;

        /// <summary>
        /// 애니메이션 갱신에 필요한 소유 캐릭터, 이동, Animator 및 지면 레이어 캐싱
        /// </summary>
        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _movement = _owner.GetComponent<Movement>();
            _boneAnimator = GetComponent<Animator>();
            _groundLayer = LayerMask.GetMask("Ground");
        }

        /// <summary>
        /// 이동 상태를 Animator 파라미터에 매 프레임 반영
        /// </summary>
        private void Update()
        {
            _boneAnimator.SetFloat("Speed", _owner.Movement.NormalizedHorizontalVelocity);
            _boneAnimator.SetBool("IsSprint", _owner.Movement.IsSprint);
            _boneAnimator.SetBool("IsJumping", _movement.IsJumping);
            _boneAnimator.SetBool("IsFalling", _movement.IsFalling);

            UpdateMoveAnimation();
        }

        /// <summary>
        /// 테스트 액션 트리거 실행
        /// </summary>
        public override void PlayActionTest()
        {
            base.PlayActionTest();
            _boneAnimator.SetTrigger("ActionTest");
        }

        /// <summary>
        /// 월드 이동 속도를 캐릭터 로컬 방향으로 바꾸어 블렌드 트리에 전달
        /// </summary>
        private void UpdateMoveAnimation()
        {
            Vector3 velocity = _owner.Movement.HorizontalVelocity;

            Vector3 localDir = Vector3.zero;

            // 실제 이동 중일 때만 전후·좌우 방향값을 계산한다.
            if (_owner.Movement.IsCanMove && velocity.sqrMagnitude > 0.001f)
            {
                localDir = transform.InverseTransformDirection(velocity.normalized);
            }

            const float DAMP_TIME = 0.1f;

            // 급격한 방향 전환에도 블렌드 값이 튀지 않도록 댐핑한다.
            _boneAnimator.SetFloat("PosX", localDir.x, DAMP_TIME, Time.deltaTime);
            _boneAnimator.SetFloat("PosY", localDir.z, DAMP_TIME, Time.deltaTime);
        }
    }
}
