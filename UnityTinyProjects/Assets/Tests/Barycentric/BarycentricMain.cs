using UnityEditor;
using UnityEngine;

public class BarycentricMain : MonoBehaviour
{
    public GameObject m_a;

    public GameObject m_b;
    public GameObject m_c;
    public GameObject m_p;
    public int m_count = 10;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        //draw tri abc
        
        // Gizmos.DrawLine(m_a.transform.position, m_b.transform.position);
        // Gizmos.DrawLine(m_b.transform.position, m_c.transform.position);
        // Gizmos.DrawLine(m_c.transform.position, m_a.transform.position);
        
        //draw labels
        Handles.Label(m_a.transform.position,"A");
        Handles.Label(m_b.transform.position,"B");
        Handles.Label(m_c.transform.position,"C");
        Handles.Label(m_p.transform.position,"P");
        
        //draw ap
        Gizmos.DrawLine(this.m_a.transform.position, this.m_p.transform.position);
        
        //draw cross product of ba and ac
        var ba = this.m_a.transform.position - this.m_b.transform.position;
        var ac = this.m_c.transform.position - this.m_a.transform.position;
        var perpabc = Vector3.Cross(ba, ac).normalized;
        var perpabcEnd= this.m_a.transform.position + perpabc*10f;
        // Draw.Arrow(this.m_a.transform.position,perpabcEnd);
        Gizmos.DrawSphere(perpabcEnd,0.5f);
        var style=new GUIStyle();
        style.normal.textColor = Color.yellow;
        Handles.Label(perpabcEnd+Vector3.up*1.5f,"ABCPerp",style);
        
        //draw p projection
        var ap= this.m_p.transform.position - this.m_a.transform.position;
        var projScale = Vector3.Dot(ap, perpabc);
        var projPointOnPerpAbc = this.m_p.transform.position - projScale * perpabc; 
        Gizmos.DrawLine(this.m_p.transform.position, projPointOnPerpAbc);
        Gizmos.DrawSphere(projPointOnPerpAbc,0.5f);
        Handles.Label(projPointOnPerpAbc+Vector3.up*1.5f,"P'",style);
        

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
