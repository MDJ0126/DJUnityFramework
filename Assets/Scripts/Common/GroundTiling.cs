using UnityEngine;

public class GroundTiling : MonoBehaviour
{
    /// <summary>
    /// 런타임 시작 시 지면 크기에 맞춰 텍스처 타일링 갱신
    /// </summary>
    private void Awake()
    {
        UpdateTexture();
    }

    /// <summary>
    /// 인스펙터에서 크기를 바꿀 때 텍스처 타일링 미리보기 갱신
    /// </summary>
    private void OnValidate()
    {
        UpdateTexture();
    }

    /// <summary>
    /// 오브젝트의 X/Z 스케일을 머티리얼 텍스처 반복 횟수로 적용
    /// </summary>
    private void UpdateTexture()
    {
        Vector3 scale = transform.localScale;
        Renderer groundRenderer = GetComponent<Renderer>();
        groundRenderer.sharedMaterial.mainTextureScale = new Vector2(scale.x, scale.z);
    }
}
