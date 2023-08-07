using Unity.Mathematics;
using UnityEngine;

public class PipeMatrixTest : MonoBehaviour
{
    
    public GameObject m_go;
    
    Matrix4x4 CalculateModel2WorldMatrix(Transform t)
    {
        var ret = float4x4.identity;
        
        var scale= new float4x4(
            t.lossyScale.x,0,0,0,
            0,t.lossyScale.y,0,0,
            0,0,t.lossyScale.z,0,
            0,0,0,1
        );
        
        var translate= new float4x4(
            1,0,0,t.position.x,
            0,1,0,t.position.y,
            0,0,1,t.position.z,
            0,0,0,1
        );

        ret = math.mul(translate,scale);
        
        return ret;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var localToWorldMatrix=m_go.transform.localToWorldMatrix;
        ConsoleProDebug.Watch("unity M2W matrix","\n"+localToWorldMatrix.ToString());
        ConsoleProDebug.Watch("my M2W matrix","\n"+CalculateModel2WorldMatrix(m_go.transform).ToString());
    }
}
