using Unity.Mathematics;
using UnityEngine;

public class ArrowTest: MonoBehaviour
{
    public GameObject m_start;
    public GameObject m_end;
    public float m_headScale=0.1f;
    public float m_angle = 30;
    public Color m_color = Color.green;
    public void OnDrawGizmos()
    {
        Gizmos.color=m_color;
        DrawArrow(m_start.transform.position,m_end.transform.position,m_headScale,m_angle);
    }

    void DrawArrow(float3 start, float3 end,float headScale= 0.1f, float angle=30f)
    {
        var dir=end-start;
        var headStart=start+dir*(1-headScale);
        var cross=math.cross(dir,math.up());
        var crossNorm=math.normalize(cross); 
        var crossShouldLen=math.length(end-headStart)*math.tan(angle*Mathf.Deg2Rad);
        cross=crossNorm*crossShouldLen;
        Gizmos.DrawLine(end,headStart+cross);
        Gizmos.DrawLine(end,headStart-cross);
        Gizmos.DrawLine(start,end);
    }
}