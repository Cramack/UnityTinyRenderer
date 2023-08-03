using UnityEngine;

[CreateAssetMenu(menuName = "Create RendererConfig", fileName = "RendererConfig", order = 0)]
public class RendererConfig : ScriptableObject 
{
    /// <summary>
    /// 是否使用Unity原生的渲染方式
    /// </summary>
    public bool m_useUnityNativeRendering = false;
    /// <summary>
    /// 默认的清屏颜色
    /// </summary>
    public Color m_clearColor = Color.black;

}
