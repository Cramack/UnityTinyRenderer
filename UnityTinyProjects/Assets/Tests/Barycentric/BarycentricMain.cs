using UnityEditor;
using UnityEngine;

public class BarycentricMain : MonoBehaviour
{
    public GameObject m_start;

    public GameObject m_end;
    public GameObject m_player;
    public int m_count = 10;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        var startColor = m_start.GetComponent<MeshRenderer>().sharedMaterial.color;
        var endColor = m_end.GetComponent<MeshRenderer>().sharedMaterial.color;
        
        Gizmos.DrawLine(m_start.transform.position, m_end.transform.position);
        Gizmos.DrawLine(m_start.transform.position, m_player.transform.position);
        Gizmos.DrawLine(m_player.transform.position, m_end.transform.position);
        
        var start2End= m_end.transform.position - m_start.transform.position;
        var start2Player= m_player.transform.position - m_start.transform.position;
        var projScale = Vector3.Dot(start2End, start2Player)/start2End.sqrMagnitude;
        var projPointOnStartEnd = m_start.transform.position + projScale * start2End;
        Gizmos.color=(1-projScale)*startColor+projScale*endColor;
        Gizmos.DrawCube(projPointOnStartEnd,Vector3.one*0.5f);
        Handles.Label(projPointOnStartEnd+Vector3.up,projScale.ToString("0.00"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
