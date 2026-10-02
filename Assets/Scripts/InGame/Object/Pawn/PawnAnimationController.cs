using UnityEngine;

namespace Game
{
    public abstract class PawnAnimationController : MonoBehaviour
    {
        private Pawn _pawn;
        protected Animator boneAnimator;

        /// <summary>
        /// 애니메이션 갱신에 필요한 소유 캐릭터, 이동, Animator 및 지면 레이어 캐싱
        /// </summary>
        protected virtual void Awake()
        {
            _pawn = GetComponentInParent<Pawn>();
            boneAnimator = GetComponent<Animator>();
        }
    }
}
