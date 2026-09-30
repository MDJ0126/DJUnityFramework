using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 인스펙터에서 변수 이름을 커스텀 문자열로 표시해주는 어트리뷰트입니다.
/// </summary>
public class DisplayNameAttribute : PropertyAttribute
{
    public string Name { get; private set; }

    /// <summary>
    /// 인스펙터에 대신 표시할 필드 이름 저장
    /// </summary>
    public DisplayNameAttribute(string name)
    {
        this.Name = name;
    }
}

#if UNITY_EDITOR
/// <summary>
/// CustomNameAttribute가 부착된 필드를 인스펙터에 그리는 에디터 스크립트입니다.
/// </summary>
[CustomPropertyDrawer(typeof(DisplayNameAttribute))]
public class DisplayNameNameDrawer : PropertyDrawer
{
    /// <summary>
    /// 기본 라벨을 어트리뷰트에 지정한 표시 이름으로 교체하여 그리기
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 어트리뷰트 타겟을 가져옴
        DisplayNameAttribute customName = (DisplayNameAttribute)attribute;

        // 기존 라벨의 텍스트를 커스텀 이름으로 교체
        label.text = customName.Name;

        // 변수 타입에 맞는 기본 유니티 GUI를 그대로 그려줌
        EditorGUI.PropertyField(position, property, label, true);
    }

    /// <summary>
    /// 배열과 복합 타입을 포함한 기본 프로퍼티 높이 반환
    /// </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 배열이나 리스트, 혹은 복잡한 구조체(클래스)일 때 높이가 깨지지 않도록 처리
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
