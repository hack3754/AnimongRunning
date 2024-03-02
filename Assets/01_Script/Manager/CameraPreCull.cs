using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraPreCull : MonoBehaviour
{
    void OnPreCull()
    {
        GL.Clear(true, true, new Color32(0x1C, 0x28, 0x3A, 0xFF));
    }
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += EndCameraRenderling;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= EndCameraRenderling;
    }

    private void EndCameraRenderling(ScriptableRenderContext context, Camera camera)
    {
        GL.Clear(true, true, new Color32(0x1C, 0x28, 0x3A, 0xFF));
    }
}
