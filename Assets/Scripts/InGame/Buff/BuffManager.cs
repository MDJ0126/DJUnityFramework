using System.Collections.Generic;

namespace Game
{
    public class BuffManager
    {
        private Pawn _owner = null;

        public List<Buff> Buffs { get; private set; } = new();

        public void SetOwner(Pawn owner)
        {
            _owner = owner;
        }

        public void UpdateTick(float deltaTime)
        {
            foreach (Buff buff in Buffs)
            {
                buff.UpdateTick(deltaTime);
            }
        }

        public void AddBuff(Buff buff)
        {
            buff.SetOwner(_owner);
            buff.OnEndBuff += RemoveBuff;
            Buffs.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);
        }
    }
}