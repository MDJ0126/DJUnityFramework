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
        public Vector3 AimTargetDefault { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AimTargetDefault = aimTarget.target.localPosition;
        }

        public void ResetAimTargetPosition()
        {
            aimTarget.target.localPosition = AimTargetDefault;
        }

        public override void Possess(PlayerController playerController)
        {
            base.Possess(playerController);
            playerController.aimTarget = aimTarget;
        }

        public override void Unpossess(PlayerController playerController)
        {
            base.Unpossess(playerController);
            ResetAimTargetPosition();
        }
    }
}