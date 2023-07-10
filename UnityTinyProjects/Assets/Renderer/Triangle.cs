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
}