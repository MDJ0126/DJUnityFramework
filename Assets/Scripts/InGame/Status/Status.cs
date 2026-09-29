namespace Game
{
    public struct Status
    {
        public int damage;
        public int defence;
        public int maxHp;

        public static Status operator +(Status a, Status b)
        {
            a.damage += b.damage;
            a.defence += b.defence;
            a.maxHp += b.maxHp;
            return a;
        }

        public static Status operator -(Status a, Status b)
        {
            a.damage -= b.damage;
            a.defence -= b.defence;
            a.maxHp -= b.maxHp;
            return a;
        }
    }
}