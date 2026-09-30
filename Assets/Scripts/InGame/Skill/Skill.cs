namespace Game
{
    public abstract class Skill
    {
        protected Pawn owner = null;

        /// <summary>
        /// 스킬을 실행할 소유 Pawn 설정
        /// </summary>
        public void SetOwner(Pawn owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// 파생 스킬의 고유 효과 실행
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// 지속형 스킬의 시간 기반 로직 확장 지점
        /// </summary>
        public virtual void UpdateTick(float deltaTime)
        {

        }
    }
}
