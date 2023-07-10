using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RawImage m_rawImage;
    public Camera m_camera;
    public RendererConfig m_renderConfig;

    private Texture2D m_texture2D;

    Color[] m_frameBuf;
    float[] m_zBuf;
    
    


    public DrawMode m_drawMode = DrawMode.All;

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
        //set up screen
        m_texture2D = new Texture2D(Screen.width, Screen.height);
        m_texture2D.filterMode = FilterMode.Point;
        m_texture2D.wrapMode = TextureWrapMode.Clamp;
        m_rawImage.texture = m_texture2D;
        m_rawImage.SetNativeSize();

        SetUpBufs();
    }

    int m_bufWidth;
    int m_bufHeight;
    void SetUpBufs()
    {
        this.m_bufWidth=Screen.width;
        this.m_bufHeight=Screen.height;
        this.m_zBuf=new float[m_bufWidth*m_bufHeight];
        this.m_frameBuf=new Color[m_bufWidth*m_bufHeight];
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
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

        var startP = 0;
        var startQ = 0;
        var endP = 0;
        var endQ = 0;
        //if steep, we use y as p, x as q; which means draw p as y and q as x
        if (steep)
        {
            startP = y0;
            startQ = x0;
            endP = y1;
            endQ = x1;
        }
        else
        {
            startP = x0;
            startQ = y0;
            endP = x1;
            endQ = y1;
        }

        //make sure startP is smaller than endP.
        if (startP > endP)
        {
            (startP, startQ, endP, endQ) = (endP, endQ, startP, startQ);
        }


        //handle degenerate cases
        if (endP == startP)
        {
            DrawPixel(startP, startQ, color);
            return;
        }

        //how q changes with p
        float k_pq = (endQ - startQ) / (float)(endP - startP);

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
        if (x < 0 || x >= this.m_bufWidth || y < 0 || y >= this.m_bufHeight)
            return;

        this.m_frameBuf[x + this.m_bufWidth * y] = color;
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

    [SerializeField]
    Light m_light;

    void DrawHeadModel()
    {
        Profiler.BeginSample("DrawHeadModel");
        
        // light direction 
        var lightReflectDir = Vector3.forward;
        
        
        var meshFilter = m_headModel.GetComponent<MeshFilter>();
        var mesh = meshFilter.sharedMesh;
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i + 1]];
            var v2 = vertices[triangles[i + 2]];
            
            //plane normal 
            var planeNormal = Vector3.Cross(v1 - v0, v2 - v0).normalized;
            
            //light intensity
            var lightIntensity = Vector3.Dot(planeNormal, lightReflectDir);
            
            if(lightIntensity<0)
                continue;
            DrawTri(v0, v1, v2, m_light.color*lightIntensity);
        }

        Profiler.EndSample();
    }
    
    

    void DrawLine(Vector3 v0, Vector3 v1, Color color)
    {
        var x0 = (int)((v0.x) * Screen.width * 0.5) + Screen.width / 2;
        ;
        var y0 = (int)((v0.y) * Screen.height * 0.5) + Screen.height / 2;
        var x1 = (int)((v1.x) * Screen.width * 0.5) + Screen.width / 2;
        var y1 = (int)((v1.y) * Screen.height * 0.5) + Screen.height / 2;
        DrawLineInPixels(x0, y0, x1, y1, color);
    }

    void DrawTri(Vector3 v0, Vector3 v1, Vector3 v2, Color color)
    {
        var x0 = (int)((v0.x) * Screen.width * 0.5) + Screen.width / 2;
        ;
        var y0 = (int)((v0.y) * Screen.height * 0.5) + Screen.height / 2;
        var x1 = (int)((v1.x) * Screen.width * 0.5) + Screen.width / 2;
        var y1 = (int)((v1.y) * Screen.height * 0.5) + Screen.height / 2;
        var x2 = (int)((v2.x) * Screen.width * 0.5) + Screen.width / 2;
        var y2 = (int)((v2.y) * Screen.height * 0.5) + Screen.height / 2;
        RenderTriInFilled(x0, y0, x1, y1, x2, y2, color);
    }

    void DrawTriInPixels(Vector2Int v0, Vector2Int v1, Vector2Int v2, Color color)
    {
        DrawLineInPixels(v0.x, v0.y, v1.x, v1.y, color);
        DrawLineInPixels(v1.x, v1.y, v2.x, v2.y, color);
        DrawLineInPixels(v2.x, v2.y, v0.x, v0.y, color);
    }

    void DrawTriInPixels(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
    {
        var v0 = new Vector2Int(x0, y0);
        var v1 = new Vector2Int(x1, y1);
        var v2 = new Vector2Int(x2, y2);
        DrawTriInPixels(v0, v1, v2, color);
    }
    
    

    void RenderTriInFilled(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
    {
        //get bbox 
        var bbox=BBoxInt2D.GetBBox(Vector2Int.zero, new Vector2Int(this.m_texture2D.width-1, this.m_texture2D.height-1), 
            new Vector2Int(x0, y0), new Vector2Int(x1, y1), new Vector2Int(x2, y2));

        var point = Vector2Int.zero;

        for (point.x= bbox.m_min.x; point.x<=bbox.m_max.x; point.x++)
        {
            for (point.y = bbox.m_min.y; point.y<=bbox.m_max.y; point.y++)
            {
                var barycentric =RenderingHelper.BaryCentric( new Vector2Int(x0, y0), new Vector2Int(x1, y1), new Vector2Int(x2, y2),point);
                if(barycentric.x<0||barycentric.y<0||barycentric.z<0)
                    continue;
                DrawPixel(point.x,point.y,color);
            } 
        }
        
        
        // List<(int X, int Y)> points = new List<(int X, int Y)>()
        // {
        //     (x0, y0),
        //     (x1, y1),
        //     (x2, y2)
        // };
        //
        // // Sort the points based on their Y value.
        // points.Sort((a, b) => b.Y.CompareTo(a.Y));
        //
        // var top = points[0];
        // var mid = points[1];
        // var bot = points[2];
        //
        // //degenerate to a point 
        // if (top.Y == bot.Y)
        // {
        //     DrawPixel(top.X, top.Y, color);
        //     return;
        // }
        //
        // var tb_k = (float)(top.X - bot.X) / (top.Y - bot.Y);
        //
        // if (top.Y != mid.Y) // if topy=midy no up triangle
        // {
        //     var tm_k = (float)(top.X - mid.X) / (top.Y - mid.Y);
        //     //draw top part
        //     for (int y = top.Y; y >= mid.Y; y--)
        //     {
        //         var tb_x = (int)(tb_k * (y - bot.Y) + bot.X);
        //         var tm_x = (int)(tm_k * (y - mid.Y) + mid.X);
        //         DrawLineInPixels(tb_x, y, tm_x, y, color);
        //     }
        // }
        //
        //
        // if (mid.Y != bot.Y) // if midy=boty no bottom triangle
        // {
        //     var mb_k = (float)(mid.X - bot.X) / (mid.Y - bot.Y);
        //     //draw bottom part
        //     for (int y = mid.Y; y >= bot.Y; y--)
        //     {
        //         var tb_x = (int)(tb_k * (y - bot.Y) + bot.X);
        //         var mb_x = (int)(mb_k * (y - bot.Y) + bot.X);
        //         DrawLineInPixels(tb_x, y, mb_x, y, color);
        //     }
        // }
    }


    void Clear()
    {
        RenderingHelper.FillArrayV2(this.m_frameBuf, m_renderConfig.m_clearColor);
        RenderingHelper.FillArrayV2(this.m_zBuf,0);
    }

    void Render()
    {
        Profiler.BeginSample("Render");

        Profiler.BeginSample("Clear");
        Clear();
        Profiler.EndSample();
        DrawHeadModel();
        
        // var v0=new Vector2Int(0,0);
        // var v1=new Vector2Int(100,100);
        // var v2=new Vector2Int(200,50);
        //
        // DrawTriInPixels(v0.x, v0.y, v1.x, v1.y, v2.x, v2.y, Color.red);
        //
        // var p = new Vector2Int(100, 50);
        // var f=RenderingHelper.BaryCentric(v0, v1, v2,p );
        // Debug.Log($"f:{f}");
        
        
        Buf2Screen();
        Profiler.EndSample();
    }

    void Buf2Screen()
    {
        //#ltd opt setpixel data
        this.m_texture2D.SetPixels(this.m_frameBuf);
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