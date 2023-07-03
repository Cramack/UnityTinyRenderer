using UnityEngine;

struct BBoxInt2D
{
    public Vector2Int m_min;
    public Vector2Int m_max;

    public static BBoxInt2D GetBBox(Vector2Int min,Vector2Int max,Vector2Int p0,Vector2Int p1,Vector2Int p2)
    {
        var ret=new BBoxInt2D();
        
        var minx = Mathf.Min(p0.x, p1.x, p2.x);
        var miny = Mathf.Min(p0.y, p1.y, p2.y);
        var maxx = Mathf.Max(p0.x, p1.x, p2.x);
        var maxy = Mathf.Max(p0.y, p1.y, p2.y);
        
        //clamp
        minx=Mathf.Max(minx,min.x);
        miny=Mathf.Max(miny,min.y);
        maxx=Mathf.Min(maxx,max.x);
        maxy=Mathf.Min(maxy,max.y);
        
        ret.m_min=new Vector2Int(minx,miny);
        ret.m_max=new Vector2Int(maxx,maxy);
        
        return ret;
    }
}