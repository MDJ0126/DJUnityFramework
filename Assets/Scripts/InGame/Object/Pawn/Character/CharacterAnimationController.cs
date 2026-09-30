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
            _boneAnimator.SetFloat("Speed", _owner.Movement.NormalizedVelocity);
            _boneAnimator.SetBool("IsSprint", _owner.Movement.IsSprint);
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
            Vector3 velocity = _owner.Movement.Velocity;
            velocity.y = 0f;

            Vector3 localDir = Vector3.zero;

            if (velocity.sqrMagnitude > 0.001f)
            {
                localDir = transform.InverseTransformDirection(velocity.normalized);
            }

            const float DAMP_TIME = 0.1f;

            _boneAnimator.SetFloat("PosX", localDir.x, DAMP_TIME, Time.deltaTime);
            _boneAnimator.SetFloat("PosY", localDir.z, DAMP_TIME, Time.deltaTime);
        }
    }
}