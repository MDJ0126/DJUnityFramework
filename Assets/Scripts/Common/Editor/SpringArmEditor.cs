using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpringArm))]
public class SpringArmEditor : Editor
{
    private void OnSceneGUI()
    {
        SpringArm springArm = (SpringArm)target;

        DrawPositionHandle(springArm);
        DrawRotationHandle(springArm);
    }

    private void DrawPositionHandle(SpringArm springArm)
    {
        EditorGUI.BeginChangeCheck();

        Vector3 newPosition = Handles.PositionHandle(
            springArm.CameraWorldPosition,
            springArm.CameraWorldRotation
        );

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(
                springArm,
                "Move SpringArm Camera"
            );

            springArm.CameraLocalPosition =
                springArm.transform.InverseTransformPoint(newPosition);

            EditorUtility.SetDirty(springArm);
        }
    }

    private void DrawRotationHandle(SpringArm springArm)
    {
        EditorGUI.BeginChangeCheck();

        Quaternion newRotation = Handles.RotationHandle(
            springArm.CameraWorldRotation,
            springArm.CameraWorldPosition
        );

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(
                springArm,
                "Rotate SpringArm Camera"
            );

            Quaternion localRotation =
                Quaternion.Inverse(springArm.transform.rotation)
                * newRotation;

            springArm.CameraLocalRotation =
                localRotation.eulerAngles;

            EditorUtility.SetDirty(springArm);
        }
    }
}