using UnityEngine;

public class GroundTiling : MonoBehaviour
{
    private void Awake()
    {
        UpdateTexture();
    }

    private void OnValidate()
    {
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        Vector3 scale = transform.localScale;
        Renderer groundRenderer = GetComponent<Renderer>();
        groundRenderer.sharedMaterial.mainTextureScale = new Vector2(scale.x, scale.z);
    }
}