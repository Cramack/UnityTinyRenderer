using UnityEngine;

public static class RenderingHelper
{
    public static Vector3 BaryCentric(Vector2Int v0, Vector2Int v1, Vector2Int v2, Vector2Int p)
    {
        var a=new Vector3(v2.x-v0.x,v1.x-v0.x,v0.x-p.x);
        var b=new Vector3(v2.y-v0.y,v1.y-v0.y,v0.y-p.y);
        //calculate cross product
        var cross=Vector3.Cross(a,b);
        
        //triangle is degenerate
        if(cross.z==0)
        {
            return new Vector3(-1,1,1);
        }
        
        return new Vector3(1-(cross.x+cross.y)/cross.z,cross.x/cross.z,cross.y/cross.z);
    }
}