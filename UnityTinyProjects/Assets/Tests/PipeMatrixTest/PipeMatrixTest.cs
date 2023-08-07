using Unity.Mathematics;
using UnityEngine;

public class PipeMatrixTest : MonoBehaviour
{
    public GameObject m_go;
    float4x4 CalculateTransform2Matrix(Transform t)
    {
        var ret = float4x4.identity;

        var scale = t.localScale;
        var scaleMatrix= new float4x4(
            scale.x,0,0,0,
            0,scale.y,0,0,
            0,0,scale.z,0,
            0,0,0,1
        );

        var rotation = t.localRotation.eulerAngles/180*math.PI;
        
        var angleAroundXAxis = rotation.x;
        var rotationAroundXAxis = new float4x4(
            1,    0,                               0,                             0,
            0,    math.cos(angleAroundXAxis),      -math.sin(angleAroundXAxis),   0,
            0,    math.sin(angleAroundXAxis),      math.cos(angleAroundXAxis),    0,
            0,    0,                               0,                             1
            );
        
        var angleAroundYAxis = rotation.y;
        var rotationAroundYAxis = new float4x4(
            math.cos(angleAroundYAxis),    0,    math.sin(angleAroundYAxis),    0,
            0,                             1,    0,                             0,
            -math.sin(angleAroundYAxis),   0,    math.cos(angleAroundYAxis),    0,
            0,                             0,    0,                             1
        );
        
        var angleAroundZAxis = rotation.z;
        var rotationAroundZAxis = new float4x4(
            math.cos(angleAroundZAxis),    -math.sin(angleAroundZAxis),     0,    0,
            math.sin(angleAroundZAxis),    math.cos(angleAroundZAxis),     0,    0,
            0,                             0,                              1,    0,
            0,                             0,                              0,    1
        );
        

        var rotationMatrix =math.mul(rotationAroundYAxis,math.mul(rotationAroundXAxis,rotationAroundZAxis)) ;


        var pos = t.localPosition;
        var translate= new float4x4(
            1,0,0,pos.x,
            0,1,0,pos.y,
            0,0,1,pos.z,
            0,0,0,1
        );

        ret =math.mul(translate,math.mul(rotationMatrix,scaleMatrix));
        
        return ret;
    }

    void OutputModelMatrixTestResult()
    {
        var localToWorldMatrix=m_go.transform.localToWorldMatrix;
        ConsoleProDebug.Watch("unity M2W matrix","\n"+localToWorldMatrix);

        var localMatrix = CalculateTransform2Matrix(m_go.transform);
        var parentMatrix= CalculateTransform2Matrix(m_go.transform.parent);
        var m2wMatrix = math.mul(parentMatrix,localMatrix);
        ConsoleProDebug.Watch("my M2W matrix","\n"+(Matrix4x4)m2wMatrix);
    }

    void OutputViewMatrixTestResult()
    {
        ConsoleProDebug.Watch("camera matrix","\n"+Camera.main.worldToCameraMatrix);
        ConsoleProDebug.Watch("my view matrix","\n"+(Matrix4x4)CalculateViewMatrix());
        
        ConsoleProDebug.Watch("camera forward",Camera.main.transform.forward.ToString());
        ConsoleProDebug.Watch("cube in camera space",Camera.main.worldToCameraMatrix.MultiplyPoint(m_go.transform.position).ToString());
    }

    float4x4 CalculateViewMatrix()
    {
        return float4x4.identity;
    }

    void Update()
    {
        OutputViewMatrixTestResult();
    }
}
