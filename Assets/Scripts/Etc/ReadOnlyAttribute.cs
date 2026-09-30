using UnityEngine;
using System;

// 참고 : https://openlevel.postype.com/post/683234
//[ReadOnly] 혹은[ReadOnly(false)] 로 사용하면 항상 수정할 수 없다.
//[ReadOnly(true)] 로 사용하면 게임이 실행중인 동안에는 수정할 수 없다.

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        // Necessary since some properties tend to collapse smaller than their content
        /// <summary>
        /// 기본 프로퍼티 필드와 같은 높이 반환
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        // Draw a disabled property field
        /// <summary>
        /// 조건에 따라 GUI를 비활성화하여 읽기 전용 필드로 표시
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public readonly bool runtimeOnly;

    /// <summary>
    /// 런타임에만 잠글지 항상 잠글지 설정
    /// </summary>
    public ReadOnlyAttribute(bool runtimeOnly = false)
    {
        this.runtimeOnly = runtimeOnly;
    }
}
