using UnityEngine;

public class SpringArm : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Vector3 cameraLocalPosition = new Vector3(0f, 1.5f, -4f);

    [SerializeField]
    private Vector3 cameraLocalRotation = new Vector3(10f, 0f, 0f);

    #endregion

    public Vector3 CameraLocalPosition
    {
        get => cameraLocalPosition;
        set => cameraLocalPosition = value;
    }

    public Vector3 CameraLocalRotation
    {
        get => cameraLocalRotation;
        set => cameraLocalRotation = value;
    }

    public Vector3 CameraWorldPosition => transform.TransformPoint(cameraLocalPosition);

    public Quaternion CameraWorldRotation => transform.rotation * Quaternion.Euler(cameraLocalRotation);

    private PlayerCameraController _playerCameraController;

    /// <summary>
    /// SpringArm에 설정된 초기 카메라 위치/회전을 적용
    /// </summary>
    public void ApplyCameraSetting(PlayerCameraController playerCameraController)
    {
        _playerCameraController = playerCameraController;
        playerCameraController.transform.SetPositionAndRotation(
            CameraWorldPosition,
            CameraWorldRotation
        );
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (_playerCameraController)
            {
                if (_playerCameraController.SpringArm.Equals(this))
                {
                    _playerCameraController.Bind(this);
                }
            }
        }
        else
        {
            Camera.main.transform.SetPositionAndRotation(
                CameraWorldPosition,
                CameraWorldRotation
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(
            transform.position,
            CameraWorldPosition
        );

        Matrix4x4 previousMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(
            CameraWorldPosition,
            CameraWorldRotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(
            Vector3.zero,
            new Vector3(0.3f, 0.2f, 0.5f)
        );

        Gizmos.DrawLine(
            Vector3.zero,
            Vector3.forward
        );

        Gizmos.matrix = previousMatrix;
    }

#endif
}