namespace Game
{
    public class StatusInfo
    {
        public delegate void OnChanegdData(StatusInfo statusInfo);
        private event OnChanegdData _onChangedHp;
        public event OnChanegdData OnChangedHp
        {
            add
            {
                _onChangedHp -= value;
                _onChangedHp += value;
            }
            remove
            {
                _onChangedHp -= value;
            }
        }

        public Status baseStatus;
        public int hp = 0;
        public float HpRatio => (float)hp / baseStatus.maxHp;
        public bool IsDead => hp <= 0;

        public void Initialize()
        {
            hp = baseStatus.maxHp;
        }

        public void SetHp(float hp)
        {
            _onChangedHp?.Invoke(this);
        }

        public void AddHp(float addHp)
        {
            SetHp(hp + addHp);
        }

        public void Damaged(float damage)
        {
            SetHp(hp + damage);
        }
    }
}