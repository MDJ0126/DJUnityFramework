using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game
{
    public abstract class Pawn : BaseObject
    {
        #region Inspector

        public Transform aimTarget;
        public MultiAimConstraint headAim;
        public MultiAimConstraint spineAim;
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
        public Vector3 AimTargetDefault { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<Movement>();
            AnimationController = GetComponentInChildren<AnimationController>();
            AimTargetDefault = aimTarget.localPosition;
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

        public void ResetAimTargetPosition()
        {
            aimTarget.localPosition = AimTargetDefault;
        }
    }
}