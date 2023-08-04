using System;

/// <summary>
/// 渲染使用的三角形数据结构
/// </summary>
public struct Triangle
{
    public Vertex m_v0;
    public Vertex m_v1;
    public Vertex m_v2;

    public Triangle(Vertex v0, Vertex v1, Vertex v2)
    {
        this.m_v0=v0;
        this.m_v1=v1;
        this.m_v2=v2;
    }

    public Vertex this[int index]
    {
        get
        {
            if(index==0) return m_v0;
            if(index==1) return m_v1;
            if(index==2) return m_v2;
            throw new IndexOutOfRangeException();
        }
        set
        {
            if(index==0) m_v0=value;
            else if(index==1) m_v1=value;
            else if (index == 2) m_v2 = value;
            else throw new IndexOutOfRangeException();
        }
    }
        

}