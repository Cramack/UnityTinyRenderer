using Drawing;
using Unity.Mathematics;
using UnityEngine;

public class Axis: MonoBehaviourGizmos
{
     public float3 m_center;
     public float3 m_dir;
     public override void DrawGizmos()
     {
          var scale = transform.localScale;
          using (Draw.InLocalSpace(transform))
          {
               
               Draw.Arrow(float3.zero, new float3(scale.x,0,0));
          }
          
     }
}