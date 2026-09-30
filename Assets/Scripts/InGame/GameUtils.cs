namespace Game
{
    public static class GameUtils
    {
        /// <summary>
        /// 버프 생성
        /// </summary>
        /// <param name="buffType"></param>
        /// <returns></returns>
        public static Buff CreateBuff(eBuffType buffType)
        {
            // Enum 값에 대응하는 구체 Buff 인스턴스를 생성하고 지원하지 않는 값은 null을 반환한다.
            switch (buffType)
            {
                case eBuffType.Heal:
                    return new HealBuff();
            }
            return null;
        }
    }
}
