using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RawImage m_rawImage;
    public Camera m_camera;
    public RendererConfig m_renderConfig;

    private Texture2D m_texture2D;

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

    //Bresenham’s Line Drawing Algorithm
    void DrawLineInPixels(int x0, int y0, int x1, int y1, Color color)
    {
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
                SetPixel(y, x, color);
            }
            else
            {
                SetPixel(x, y, color);
            }

            error2 += derror2;
            if (error2 > dx)
            {
                y += (y1 > y0 ? 1 : -1);
                error2 -= dx * 2;
            }
        }
    }


    void SetPixel(int x, int y, Color color)
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
    
    void RenderTestFillTriangles()
    {
        
    }

    void Render()
    {
        // DrawHeadModel();
        RenderTestDrawWireTriangles();
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