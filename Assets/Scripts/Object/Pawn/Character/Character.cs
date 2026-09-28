using UnityEngine;

namespace Game
{
    public class Character : Pawn
    {
        public SkillManager SkillManager { get; private set; } = new();
    }
}