using UnityEngine;

namespace Game
{
    public abstract class Movement : MonoBehaviour
    {
        #region Inspector

        public float maxSpeed = 6f;
        public float acceleration = 20.48f;
        public float deceleration = 20.48f;

        #endregion

        protected Rigidbody rigidbody;

        public Vector3 MoveInput { get; set; } = Vector3.zero;
        public Vector3 Velocity { get; set; } = Vector3.zero;

        public Vector3 NormalizedVelocity => Velocity / maxSpeed;

        protected virtual void Awake()
        {
            rigidbody = GetComponentInChildren<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            Vector3 targetVelocity = MoveInput * maxSpeed;

            float accel = MoveInput.sqrMagnitude > 0f ? acceleration : deceleration;
            Velocity = Vector3.MoveTowards(Velocity, targetVelocity, accel * Time.fixedDeltaTime);
            rigidbody.linearVelocity = new Vector3(Velocity.x, rigidbody.linearVelocity.y, Velocity.z);
        }
    }
}
