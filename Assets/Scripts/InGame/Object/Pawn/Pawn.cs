using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Pawn : BaseObject
    {
        #region Inspector

        public WidgetAnchor nameAnchor;
        public WidgetAnchor balloonAnchor;
        public WidgetAnchor healthBarAnchor;

        #endregion

        public StatusInfo StatusInfo = new();
        public SkillManager SkillManager { get; private set; } = new();
        public BuffManager BuffManager { get; private set; } = new();
        public Movement Movement { get; private set; }
        public AnimationController AnimationController { get; private set; }
        protected List<FollowHUD> followHUDs = new();

        /// <summary>
        /// Pawn이 사용할 이동 및 애니메이션 컴포넌트 캐싱
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<Movement>();
            AnimationController = GetComponentInChildren<AnimationController>();
        }

        /// <summary>
        /// 스킬과 버프 관리자에 소유 Pawn 설정
        /// </summary>
        protected override void Start()
        {
            base.Start();
            SkillManager.SetOwner(this);
            BuffManager.SetOwner(this);
        }

        /// <summary>
        /// 스킬과 버프의 시간 기반 로직 갱신
        /// </summary>
        protected override void LateUpdate()
        {
            base.LateUpdate();
            SkillManager.UpdateTick(Time.deltaTime);
            BuffManager.UpdateTick(Time.deltaTime);
        }

        /// <summary>
        /// 플레이어 컨트롤러 빙의 시 파생 Pawn에서 사용할 처리 지점
        /// </summary>
        public virtual void Possess(PlayerController playerController)
        {

        }

        /// <summary>
        /// 플레이어 컨트롤러 빙의 해제 시 파생 Pawn에서 사용할 처리 지점
        /// </summary>
        public virtual void Unpossess(PlayerController playerController)
        {

        }
    }
}
