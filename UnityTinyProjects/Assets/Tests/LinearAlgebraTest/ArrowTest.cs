using System;
using Lex;
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
        Draw.DrawArrow(m_start.transform.position,m_end.transform.position,m_headScale,m_angle);
    }
}