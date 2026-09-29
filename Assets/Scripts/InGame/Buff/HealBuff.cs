namespace Game
{
    public class HealBuff : Buff
    {
        public override void UpdateTick(float deltaTime)
        {
            base.UpdateTick(deltaTime);
            owner.StatusInfo.hp += 10;
        }
    }
}