using Lex;
using UnityEditor;
using UnityEngine;

public class Axis: MonoBehaviour
{
    public GameObject m_start;
    public GameObject m_end;
    public float m_headScale=0.1f;
    public float m_angle = 30;
    public void OnDrawGizmos()
    {
        Draw.Arrow(m_start.transform.position,m_end.transform.position);
        
        var dir=m_end.transform.position-m_start.transform.position;
        var len=dir.magnitude;
        Handles.Label(this.m_start.transform.position+Vector3.up, len.ToString("0.00"));
        
        Gizmos.color=Color.red;
        Gizmos.DrawCube(m_start.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
        
        var cross=Vector3.Cross(dir, Vector3.up);
        Gizmos.color=Color.blue;
        Gizmos.DrawRay(this.m_end.transform.position,cross.normalized);
    }
}