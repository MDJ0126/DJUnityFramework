namespace Game
{
    public class RobotKyle : Character
    {
        #region Inspector

        public Socket leftHandleSocket;
        public Socket rightHandleSocket;

        #endregion
        public override string Name => nameof(RobotKyle);

        public RobotKyleAnimationController RobotKyleAnimationController { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            RobotKyleAnimationController = GetComponentInChildren<RobotKyleAnimationController>();
        }
    }
}