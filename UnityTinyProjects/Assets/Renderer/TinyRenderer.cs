using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    public RendererConfig m_config;
    public RawImage m_rawImage;
    Camera m_camera;
    
    private bool m_lastUseUnityNativeRendering;

    void Start()
    {
        Init();
        

        OnOffUnityRendering();
    }

    void Init()
    {
        
        m_lastUseUnityNativeRendering=m_config.m_useUnityNativeRendering;
        
        m_camera = GetComponent<Camera>();
        
        //setup raw img
        
    }

    void OnOffUnityRendering()
    {
        if (m_config.m_useUnityNativeRendering)
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
    void OnPostRender()
    {
        if (!m_config.m_useUnityNativeRendering)
        {
            Render();
        }
        
        if(m_lastUseUnityNativeRendering!=m_config.m_useUnityNativeRendering){
            m_lastUseUnityNativeRendering=m_config.m_useUnityNativeRendering;
            OnOffUnityRendering();
        }
        
    }
}