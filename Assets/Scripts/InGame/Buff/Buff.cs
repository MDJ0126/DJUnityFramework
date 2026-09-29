namespace Game
{
    public abstract class Buff
    {
        public delegate void OnChangedState(Buff buff);
        private event OnChangedState _onEndBuff;
        public event OnChangedState OnEndBuff
        {
            add
            {
                _onEndBuff -= value;
                _onEndBuff += value;
            }
            remove
            {
                _onEndBuff -= value;
            }
        }

        protected Pawn owner = null;
        protected float time = 0f;
        protected float duration = 0f;

        public void SetOwner(Pawn owner)
        {
            this.owner = owner;
        }

        public virtual void UpdateTick(float deltaTime)
        {
            if (duration >= time)
            {
                duration = time;
                _onEndBuff?.Invoke(this);
                _onEndBuff = null;
                return;
            }
            duration += deltaTime;
        }
    }
}