using Lex;
using UnityEngine;

public class Axis: MonoBehaviour
{
    public bool m_worldMode = true;
    public int m_length = 1;
    public float m_headScale = 0.1f;
    public float m_arrowAngle=30f;
    
    void OnDrawGizmos()
    {
        if (!m_worldMode)
        {
            Gizmos.matrix=transform.localToWorldMatrix;
        }
        
        //up arrow
        Gizmos.color=Color.green;
        Draw.DrawArrow(transform.position,transform.position+Vector3.up*m_length,m_headScale,m_arrowAngle);
        
        //right arrow
        Gizmos.color=Color.red;
        Draw.DrawArrow(transform.position,transform.position+Vector3.right*m_length,m_headScale,m_arrowAngle);
        
        //forward arrow
        Gizmos.color=Color.blue;
        Draw.DrawArrow(transform.position,transform.position+Vector3.forward*m_length,m_headScale,m_arrowAngle);
        
    }
}