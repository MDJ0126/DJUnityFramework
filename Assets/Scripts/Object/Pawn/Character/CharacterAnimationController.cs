using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public partial class CharacterAnimationController : MonoBehaviour
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
            _boneAnimator.SetFloat("Velocity", _owner.Movement.NormalizedVelocity.magnitude);
            _boneAnimator.SetBool("IsJumping", _movement.IsJumping);
            _boneAnimator.SetBool("IsFalling", _movement.IsFalling);
        }
    }
}