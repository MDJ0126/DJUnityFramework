using UnityEngine;

namespace Game
{
    public abstract class BaseObject : MonoBehaviour
    {
        private static int _createIndex = 0;
        public int Index { get; private set; } = 0;

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

        /// <summary>
        /// 생성 순서에 따라 오브젝트 고유 인덱스 할당
        /// </summary>
        protected virtual void Awake() 
        {
            Index = _createIndex++;
        }

        /// <summary>
        /// 파생 오브젝트의 시작 처리 확장 지점
        /// </summary>
        protected virtual void Start() { }
        /// <summary>
        /// 파생 오브젝트의 활성화 처리 확장 지점
        /// </summary>
        protected virtual void OnEnable() { }
        /// <summary>
        /// 파생 오브젝트의 비활성화 처리 확장 지점
        /// </summary>
        protected virtual void OnDisable() { }
        /// <summary>
        /// 파생 오브젝트의 매 프레임 처리 확장 지점
        /// </summary>
        protected virtual void Update() { }
        /// <summary>
        /// 파생 오브젝트의 프레임 후처리 확장 지점
        /// </summary>
        protected virtual void LateUpdate() { }
    }
}
