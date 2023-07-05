using Lex;
using UnityEngine;

public class Axis: MonoBehaviour
{
    public GameObject m_start;
    public GameObject m_end;
    
    public void OnDrawGizmos()
    {
        Draw.Arrow(m_start.transform.position,m_end.transform.position);
    }
}