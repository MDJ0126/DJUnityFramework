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

        /// <summary>
        /// 버프 효과를 적용할 소유 Pawn 설정
        /// </summary>
        public void SetOwner(Pawn owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// duration과 누적 time을 비교하고 버프 종료 이벤트 호출
        /// </summary>
        public virtual void UpdateTick(float deltaTime)
        {
            // 현재 구현은 duration이 누적 time 이상이면 종료를 알리고 구독자를 비운다.
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
