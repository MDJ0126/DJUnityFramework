using System.Collections;
using UnityEngine;

namespace Game
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        public bool IsPossessed { get; private set; } = false;
        private Pawn _possessTarget = null;
        private Pawn _lastPossessTarget = null;

        private Transform _mainCameraTransform;
        private Movement _movement;

        private void Start()
        {
            _mainCameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (!IsPossessed) return;

            if (_movement)
            {
                // 이동
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 cameraForward = Vector3.ProjectOnPlane(_mainCameraTransform.forward, Vector3.up).normalized;
                Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward).normalized;
                Vector2 input = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
                _movement.MoveInput = cameraForward * input.y + cameraRight * input.x;

                // 점프
                bool isJump = Input.GetKeyUp(KeyCode.Space);
                if (isJump)
                {
                    _movement.Jump();
                }

                // 좌클릭



                // 우클릭
            }
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

            _possessTarget = pawn;
            _lastPossessTarget = pawn;
            _movement = _possessTarget.GetComponent<Movement>();
            var playerCameraController = Camera.main.GetComponent<PlayerCameraController>();
            playerCameraController.Bind(pawn.GetComponentInChildren<SpringArm>());
            IsPossessed = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// 빙의 해제
        /// </summary>
        public void Unpossess()
        {
            _movement = null;
            IsPossessed = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}