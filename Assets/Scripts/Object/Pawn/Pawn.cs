using UnityEngine;

namespace Game
{
    public class Pawn : BaseObject
    {
        public Movement Movement { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<Movement>();
        }
    }
}