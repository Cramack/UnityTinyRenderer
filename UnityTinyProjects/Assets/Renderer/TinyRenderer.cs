using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RawImage m_rawImage;
    public Camera m_camera;
    public bool m_useNativeUnityRendering;
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
        m_rawImage.texture = m_texture2D;
        m_rawImage.SetNativeSize();
    }


    void Init()
    {
        SetupRenderingEnv();
    }

    //Bresenham’s Line Drawing Algorithm
    void DrawLine(int x0, int y0, int x1, int y1, Color color)
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
        if (m_useNativeUnityRendering)
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
        var vertices = mesh.vertices;

        foreach (var vertex in vertices)
        {
        }

      
    }

    void Render()
    {
        // DrawHeadModel();
        this.m_texture2D.SetPixel(0,0,Color.red);
        this.m_texture2D.SetPixel(Screen.width-1,Screen.height-1,Color.blue);
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