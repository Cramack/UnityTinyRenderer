using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RawImage m_rawImage;
    public Camera m_camera;
    public bool m_useNativeUnityRendering;
   private Texture2D m_texture2D;

    void Start()
    {
        Init();
        OnOffUnityNativeRendering();
    }

    void Init()
    {
        //setup raw img
        m_texture2D = new Texture2D(Screen.width, Screen.height);
        Debug.Log("Screen.width:" + Screen.width + " Screen.height:" + Screen.height);
        // m_rawImage.texture=
        
        m_rawImage.texture = m_texture2D;
        m_rawImage.SetNativeSize();
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

    // Update is called once per frame
    void Update()
    {
    }

    void Render()
    {
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