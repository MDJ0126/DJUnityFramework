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

        public static implicit operator Transform(WidgetAnchor anchor)
        {
            return anchor.Transform;
        }

#if UNITY_EDITOR
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