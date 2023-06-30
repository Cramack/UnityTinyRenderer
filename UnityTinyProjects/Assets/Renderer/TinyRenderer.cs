using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RawImage m_rawImage;
    public Camera m_camera;
    public RendererConfig m_renderConfig;

    private Texture2D m_texture2D;


    public DrawMode m_drawMode=DrawMode.All;
    
    public enum DrawMode 
    {
        Line,
        Filled,
        All
    }
    

    [SerializeField]
    GameObject m_headModel;


    void Start()
    {
        Init();
        OnOffUnityNativeRendering();
    }

    void SetupRenderingEnv()
    {
        Debug.Log("Screen.width:" + Screen.width + " Screen.height:" + Screen.height);
        m_texture2D = new Texture2D(Screen.width, Screen.height);
        m_texture2D.filterMode = FilterMode.Point;
        m_texture2D.wrapMode= TextureWrapMode.Clamp;
        m_rawImage.texture = m_texture2D;
        m_rawImage.SetNativeSize();
    }


    void Init()
    {
        SetupRenderingEnv();
    }

    public bool m_useMyLineDrawing = false;
    
    void DrawLineInPixelsV1(int x0, int y0, int x1, int y1, Color color)
    {
        //#ltd handle degenerate cases
        
        //steep means y change is bigger than x change
        bool steep = Math.Abs(y1-y0)>Math.Abs(x1-x0);
        
        var startP =0;
        var startQ=0;
        var endP=0;
        var endQ=0;
        //if steep, we use y as p, x as q; which means draw p as y and q as x
        if (steep)
        {
            startP=y0; startQ=x0;endP=y1;endQ=x1;
        }
        else
        {
            startP=x0; startQ=y0;endP=x1;endQ=y1;
        }

        //make sure startP is smaller than endP.
        if (startP> endP)
        {
            (startP,startQ)=(endP,endQ);
        }


        //handle degenerate cases
        if (endP == startP)
        {
            DrawPixel(startP,startQ, color);
            return;
        }
        
        //how q changes with p
        float k_pq=(endQ-startQ)/(float)(endP-startP);
        
        for (int p = startP; p <= endP; p++)
        {
            int q = (int)(k_pq * (p - startP) + startQ);
            if (steep)
            {
                DrawPixel(q, p, color);
            }
            else
            {
                DrawPixel(p, q, color);
            }
        }
    }

    //Bresenham’s Line Drawing Algorithm
    void DrawLineInPixels(int x0, int y0, int x1, int y1, Color color)
    {
        if (m_useMyLineDrawing)
        {
            DrawLineInPixelsV1(x0, y0, x1, y1, color);
            return;
        }
        
        bool steep = false;
        if (Mathf.Abs(x0 - x1) < Mathf.Abs(y0 - y1))
        {
            //swap x0,y0
            int temp = x0;
            x0 = y0;
            y0 = temp;

            //swap x1,y1
            temp = x1;
            x1 = y1;
            y1 = temp;

            steep = true;
        }

        if (x0 > x1)
        {
            //swap x0,x1
            int temp = x0;
            x0 = x1;
            x1 = temp;

            //swap y0,y1
            temp = y0;
            y0 = y1;
            y1 = temp;
        }

        int dx = x1 - x0;
        int dy = y1 - y0;
        int derror2 = Mathf.Abs(dy) * 2;
        int error2 = 0;
        int y = y0;
        for (int x = x0; x <= x1; x++)
        {
            if (steep)
            {
                DrawPixel(y, x, color);
            }
            else
            {
                DrawPixel(x, y, color);
            }

            error2 += derror2;
            if (error2 > dx)
            {
                y += (y1 > y0 ? 1 : -1);
                error2 -= dx * 2;
            }
        }
    }


    void DrawPixel(int x, int y, Color color)
    {
        this.m_texture2D.SetPixel(x, y, color);
    }

    /// <summary>
    /// 开关Unity原生渲染
    /// </summary>
    void OnOffUnityNativeRendering()
    {
        if (m_renderConfig.m_useUnityNativeRendering)
        {
            //render all layers
            m_camera.cullingMask = -1;
            m_rawImage.gameObject.SetActive(false);
        }
        else
        {
            //render nothing
            m_camera.cullingMask = 0;
            m_rawImage.gameObject.SetActive(true);
        }
    }


    void DrawHeadModel()
    {
        var meshFilter = m_headModel.GetComponent<MeshFilter>();
        var mesh = meshFilter.sharedMesh;
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        for (int i = 0; i < triangles.Length; i+=3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i + 1]];
            var v2 = vertices[triangles[i + 2]];
            DrawTri(v0,v1,v2);
        }
    }

    void DrawLine(Vector3 v0, Vector3 v1,Color color)
    {
        var x0=(int)((v0.x)*Screen.width*0.5)+Screen.width/2;; 
        var y0=(int)((v0.y)*Screen.height*0.5)+Screen.height/2;
        var x1=(int)((v1.x)*Screen.width*0.5)+Screen.width/2;
        var y1=(int)((v1.y)*Screen.height*0.5)+Screen.height/2;
        DrawLineInPixels(x0,y0,x1,y1,color);
    }
    
    void DrawTri(Vector3 v0,Vector3 v1,Vector3 v2)
    {
        // get random color 
        DrawLine(v0,v1,Color.black);   
        DrawLine(v1,v2,Color.black);
        DrawLine(v2,v0,Color.black);
    }

    void DrawTriInPixels(Vector2Int v0,Vector2Int v1,Vector2Int v2,Color color)
    {
        DrawLineInPixels(v0.x,v0.y,v1.x,v1.y,color);
        DrawLineInPixels(v1.x,v1.y,v2.x,v2.y,color);
        DrawLineInPixels(v2.x,v2.y,v0.x,v0.y,color);
    }

    void DrawTriInPixels(int x0, int y0, int x1, int y1, int x2, int y2,Color color)
    {
        var v0 = new Vector2Int(x0, y0); 
        var v1 = new Vector2Int(x1, y1);
        var v2 = new Vector2Int(x2, y2);
        DrawTriInPixels(v0,v1,v2,color);
    }
    
    void RenderTestDrawWireTriangles()
    {
        DrawTriInPixels(10,70,50,160,70,80,Color.red);
        DrawTriInPixels(180,50,150,1,70,180,Color.white);
        DrawTriInPixels(180,150,120,160,130,180,Color.green);
    }

    void DrawTriInFilled(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
    {
        List<(int X, int Y)> points = new List<(int X, int Y)>()
        {
            (x0, y0),
            (x1, y1),
            (x2, y2)
        };

        // Sort the points based on their Y value.
        points.Sort((a, b) => b.Y.CompareTo(a.Y));
        
        var top = points[0];
        var mid = points[1];
        var bot = points[2];
        //draw top part
        for (int y = top.Y; y>=mid.Y; y--)
        {
            DrawPixel(top.X,y,color);
        }
        
    }

    
    void RenderTestFillTriangles()
    {
        DrawTriInFilled(10,70,50,160,70,80,Color.red);
        DrawTriInFilled(180,50,150,1,70,180,Color.white);
        DrawTriInFilled(180,150,120,160,130,180,Color.green);
        
    }

    void Clear()
    {
        for (int i = 0; i < m_texture2D.width; i++)
        {
            for (int j = 0; j < m_texture2D.height; j++)
            {
                m_texture2D.SetPixel(i,j,m_renderConfig.m_clearColor);
            }
        } 
    }

    public bool m_drawGreenLine = false;

    void TestLineDrawing()
    {
        DrawLineInPixels(13,20,80,40,Color.blue);
        DrawLineInPixels(20,13,40,80,Color.red);
        if (m_drawGreenLine)
        {
            DrawLineInPixels(80,40,13,20,Color.green);
        }
        
        //draw a horizontal line
        DrawLineInPixels(100,20,200,20,Color.cyan);
        
        //draw a vertical line
        DrawLineInPixels(150,20,150,120,Color.yellow);
        
        //draw a point 
        DrawLineInPixels(200,200,200,200,Color.magenta);
    }
    
    void Render()
    {
        Clear(); 
        TestLineDrawing();
        
        // if (m_drawMode == DrawMode.Line)
        // {
        //     RenderTestDrawWireTriangles();
        // }
        // else if (m_drawMode==DrawMode.Filled)
        // {
        //     RenderTestFillTriangles();
        // }
        // else
        // {
        //     RenderTestDrawWireTriangles();
        //     RenderTestFillTriangles();
        // }
        
        this.m_texture2D.Apply();
    }

    /// <summary>
    /// 只有attach到camera上才会调用
    /// </summary>
    void OnPostRender()
    {
        Render();

        OnOffUnityNativeRendering();
    }
}