using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public Pawn _possessTarget = null;

        private Movement _movement;

        private void Start()
        {
            Possess(_possessTarget);
        }

        /// <summary>
        /// 빙의
        /// </summary>
        public void Possess(Pawn pawn)
        {
            Unpossess();
            _possessTarget = pawn;
            _movement = _possessTarget.GetComponent<Movement>();
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public void Unpossess()
        {
            _movement = null;
        }

        protected virtual void Update()
        {
            if (!_possessTarget) return;

            if (_movement)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                _movement.MoveInput = new Vector3(horizontal, 0f, vertical);
            }
        }
    }
}