namespace Game
{
    public class RobotKyle : Character
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            followHUDs.Add(HUDManager.Instance.AttachFollowName(this, nameof(RobotKyle)));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (HUDManager.IsLive)
            {
                HUDManager.Instance.DetachFollowHUD(followHUDs);
            }
        }
    }
}