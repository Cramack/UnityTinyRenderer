using Lex;
using Unity.Mathematics;
using UnityEngine;

public class Axis: MonoBehaviour
{
    void OnDrawGizmos()
    {
        //up arrow
        Gizmos.color=Color.green;
        Draw.DrawArrow(transform.position,transform.position+Vector3.up,0.1f,30f);
        
        //right arrow
        Gizmos.color=Color.red;
        Draw.DrawArrow(transform.position,transform.position+Vector3.right,0.1f,30f);
        
        //forward arrow
        Gizmos.color=Color.blue;
        Draw.DrawArrow(transform.position,transform.position+Vector3.forward,0.1f,30f);
        
    }
}