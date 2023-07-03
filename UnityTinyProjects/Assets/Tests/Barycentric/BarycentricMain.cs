using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class BarycentricMain : MonoBehaviour
{
    public GameObject m_a;

    public GameObject m_b;
    public GameObject m_c;
    public GameObject m_player;
    public int m_count = 10;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        //draw tri abc
        Gizmos.DrawLine(m_a.transform.position, m_b.transform.position);
        Gizmos.DrawLine(m_b.transform.position, m_c.transform.position);
        Gizmos.DrawLine(m_c.transform.position, m_a.transform.position);
        
        
        
        // var startColor = m_a.GetComponent<MeshRenderer>().sharedMaterial.color;
        // var endColor = m_b.GetComponent<MeshRenderer>().sharedMaterial.color;
        //
        // Gizmos.DrawLine(m_a.transform.position, m_b.transform.position);
        // Gizmos.DrawLine(m_a.transform.position, m_player.transform.position);
        // Gizmos.DrawLine(m_player.transform.position, m_b.transform.position);
        //
        // var start2End= m_b.transform.position - m_a.transform.position;
        // var start2Player= m_player.transform.position - m_a.transform.position;
        // var projScale = Vector3.Dot(start2End, start2Player)/start2End.sqrMagnitude;
        // var projPointOnStartEnd = m_a.transform.position + projScale * start2End;
        // Gizmos.color=(1-projScale)*startColor+projScale*endColor;
        // Gizmos.DrawCube(projPointOnStartEnd,Vector3.one*0.5f);
        // Handles.Label(projPointOnStartEnd+Vector3.up,projScale.ToString("0.00"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
