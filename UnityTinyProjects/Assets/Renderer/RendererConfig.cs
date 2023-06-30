using UnityEngine;

[CreateAssetMenu(menuName = "Create RendererConfig", fileName = "RendererConfig", order = 0)]
public class RendererConfig : ScriptableObject 
{
    public bool m_useUnityNativeRendering = false;
    public Color m_clearColor = Color.black;

}
