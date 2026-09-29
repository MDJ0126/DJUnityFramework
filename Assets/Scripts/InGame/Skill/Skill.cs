namespace Game
{
    public abstract class Skill
    {
        protected Pawn owner = null;

        public void SetOwner(Pawn owner)
        {
            this.owner = owner;
        }

        public abstract void Execute();
        public virtual void UpdateTick(float deltaTime)
        {

        }
    }
}