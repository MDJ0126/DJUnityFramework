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
            switch (buffType)
            {
                case eBuffType.Heal:
                    return new HealBuff();
            }
            return null;
        }
    }
}