using Unity.Mathematics;

/// <summary>
/// 顶点的数据结构
/// </summary>
public struct Vertex
{
    /// <summary>
    /// 屏幕坐标
    /// </summary>
    public float3 m_screenPos;
    /// <summary>
    /// 模型空间坐标
    /// </summary>
    public float3 m_objectPos;
    
    /// <summary>
    /// uv坐标
    /// </summary>
    public float2 m_uv;

}