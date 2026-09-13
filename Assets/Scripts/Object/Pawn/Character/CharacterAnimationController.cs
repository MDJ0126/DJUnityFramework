using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour
    {
        private Character _owner;
        private Animator _boneAnimator;

        private void Awake()
        {
            _owner = GetComponentInParent<Character>();
            _boneAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            _boneAnimator.SetFloat("Velocity", _owner.Movement.NormalizedVelocity.magnitude);
        }
    }
}