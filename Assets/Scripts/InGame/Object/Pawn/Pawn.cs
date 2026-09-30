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

        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<Movement>();
            AnimationController = GetComponentInChildren<AnimationController>();
        }

        protected override void Start()
        {
            base.Start();
            SkillManager.SetOwner(this);
            BuffManager.SetOwner(this);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            SkillManager.UpdateTick(Time.deltaTime);
            BuffManager.UpdateTick(Time.deltaTime);
        }

        public virtual void Possess(PlayerController playerController)
        {

        }

        public virtual void Unpossess(PlayerController playerController)
        {

        }
    }
}