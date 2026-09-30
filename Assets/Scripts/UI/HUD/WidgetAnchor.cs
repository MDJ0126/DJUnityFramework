using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class WidgetAnchor : MonoBehaviour
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

        /// <summary>
        /// WidgetAnchor를 HUD 타겟으로 바로 전달할 수 있도록 Transform으로 변환
        /// </summary>
        public static implicit operator Transform(WidgetAnchor anchor)
        {
            return anchor.Transform;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 씬 뷰에서 HUD 앵커의 오브젝트 이름 표시
        /// </summary>
        private void OnDrawGizmos()
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            Handles.Label(transform.position, gameObject.name, style);
        }
#endif
    }
}
