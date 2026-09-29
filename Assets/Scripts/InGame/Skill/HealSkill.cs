namespace Game
{
    public class HealSkill : Skill
    {
        public override void Execute()
        {
            owner.StatusInfo.hp += 10;
        }
    }
}