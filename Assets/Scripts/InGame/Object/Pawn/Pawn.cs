using NUnit.Framework;
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
        public Movement Movement { get; set; }
        protected List<FollowHUD> followHUDs = new();

        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<Movement>();
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
    }
}