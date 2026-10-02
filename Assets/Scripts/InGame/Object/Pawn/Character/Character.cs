using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game
{
    public abstract class Character : Pawn
    {
        #region Inspector

        public AimTarget aimTarget;
        public MultiAimConstraint headAim;
        public MultiAimConstraint spineAim;

        #endregion
        public CharacterAnimationController CharacterAnimationController { get; private set; }
        public Vector3 AimTargetDefault { get; private set; }

        /// <summary>
        /// 에임 타겟을 원래 위치로 되돌릴 수 있도록 초기 로컬 위치 저장
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            AimTargetDefault = aimTarget.target.localPosition;
            CharacterAnimationController = GetComponent<CharacterAnimationController>();
        }

        /// <summary>
        /// 캐릭터가 활성화되면 이름 HUD 생성 및 추적 목록 등록
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            followHUDs.Add(HUDManager.Instance.AttachFollowName(this, nameof(RobotKyle)));
        }

        /// <summary>
        /// 캐릭터가 비활성화되면 연결된 추적 HUD 모두 반환
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            if (HUDManager.IsLive)
            {
                HUDManager.Instance.DetachFollowHUD(followHUDs);
            }
        }

        /// <summary>
        /// 에임 타겟을 캐릭터의 초기 로컬 위치로 복원
        /// </summary>
        public void ResetAimTargetPosition()
        {
            aimTarget.target.localPosition = AimTargetDefault;
        }

        /// <summary>
        /// 빙의한 컨트롤러가 캐릭터의 에임 타겟을 조작하도록 연결
        /// </summary>
        public override void Possess(PlayerController playerController)
        {
            base.Possess(playerController);
            playerController.aimTarget = aimTarget;
        }

        /// <summary>
        /// 빙의 해제 시 변경된 에임 타겟 위치 복원
        /// </summary>
        public override void Unpossess(PlayerController playerController)
        {
            base.Unpossess(playerController);
            ResetAimTargetPosition();
        }
    }
}
