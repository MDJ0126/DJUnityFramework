using UnityEngine;

namespace Game
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        public bool IsPossessed { get; private set; } = false;
        public Pawn _possessTarget = null;
        public Pawn _lastPossessTarget = null;

        private Transform _mainCameraTransform;
        private Movement _movement;

        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
            Possess(_possessTarget);
        }

        /// <summary>
        /// 빙의
        /// </summary>
        public void Possess(Pawn pawn = null)
        {
            if (pawn == null)
            {
                pawn = _lastPossessTarget;
            }

            Unpossess();
            _possessTarget = pawn;
            _lastPossessTarget = pawn;
            _movement = _possessTarget.GetComponent<Movement>();
            var playerCameraController = Camera.main.GetComponent<PlayerCameraController>();
            playerCameraController.Bind(pawn.GetComponentInChildren<SpringArm>());
            IsPossessed = true;
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public void Unpossess()
        {
            _movement = null;
            IsPossessed = false;
        }

        protected virtual void Update()
        {
            if (!_possessTarget) return;

            if (_movement)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 cameraForward = Vector3.ProjectOnPlane(_mainCameraTransform.forward, Vector3.up).normalized;
                Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
                Vector2 input = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);

                _movement.MoveInput = cameraForward * input.y + cameraRight * input.x;

                bool isJump = Input.GetKeyUp(KeyCode.Space);
                if (isJump)
                {
                    _movement.Jump();
                }
            }
        }
    }
}