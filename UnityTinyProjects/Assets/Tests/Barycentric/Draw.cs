
using Unity.Mathematics;
using UnityEngine;

public static class Draw
{
    /// <summary>
    /// 当up值和dir接近平行时,arrow会和dir接近平行
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void Arrow(float3 from,float3 to)
    {
        Debug.DrawLine(from,to);
        var dir = to - from;
        var up = new float3(0,1,0);
        var normal = math.cross(dir, up);
        Debug.Log(math.length(normal)/math.length(dir));
        
        if(math.all(normal==0)) up=new float3(0,0,1);
        normal = math.cross(dir, up);
        
        Debug.DrawRay(from,dir+normal);
        
        var lenScale = 0.2f;
        //left side reverse when upward
        Debug.DrawLine(to,to -(dir+normal)*lenScale);
        //right side reverse when upward
        Debug.DrawLine(to,to -(dir-normal)*lenScale);
    }
}