using UnityEngine;

namespace Game
{
    public class BaseObject : MonoBehaviour
    {
        private Transform _transform;

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;
                return _transform;
            }
        }

        protected virtual void Awake()
        {
        }
    }
}