using UnityEngine;

public class RenderTextureOptimizer : MonoBehaviour
{
    public Camera camTarget;

    private void OnWillRenderObject()
    {
        camTarget.Render();
    }
}
