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

        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _movement = _owner.GetComponent<Movement>();
            _boneAnimator = GetComponent<Animator>();
            _groundLayer = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            _boneAnimator.SetFloat("Velocity", _owner.Movement.NormalizedVelocity);
            _boneAnimator.SetBool("IsJumping", _movement.IsJumping);
            _boneAnimator.SetBool("IsFalling", _movement.IsFalling);

            UpdateMoveAnimation();
        }

        public override void PlayActionTest()
        {
            base.PlayActionTest();
            _boneAnimator.SetTrigger("ActionTest");
        }

        private void UpdateMoveAnimation()
        {
            Vector3 forward = transform.forward;

            // 상하 조준은 이동 애니메이션 방향에 필요 없으므로 제거
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, forward);

            Vector3 moveDir = Camera.main.transform.forward.normalized;

            float posX = Vector3.Dot(moveDir, right);
            float posY = Vector3.Dot(moveDir, forward);

            _boneAnimator.SetFloat("PosX", posX);
            _boneAnimator.SetFloat("PosY", posY);
        }
    }
}