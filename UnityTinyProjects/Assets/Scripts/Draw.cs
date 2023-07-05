using Unity.Mathematics;
using UnityEngine;

namespace Lex
{
    public static class Draw
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="headScale">Arrow投影到直线上的scale</param>
        /// <param name="angle">Arrow开关的角度</param>
        public static void DrawArrow(float3 start, float3 end, float headScale = 0.1f, float angle = 30f)
        {
            var dir = end - start;
            var headStart = start + dir * (1 - headScale);
            var cross = math.cross(dir, math.up());
            if (math.all(cross == float3.zero))
                cross = math.cross(dir, math.right());
            
            var crossNorm = math.normalize(cross);
            var crossShouldLen = math.length(end - headStart) * math.tan(angle * Mathf.Deg2Rad);
            cross = crossNorm * crossShouldLen;
            Gizmos.DrawLine(end, headStart + cross);
            Gizmos.DrawLine(end, headStart - cross);
            Gizmos.DrawLine(start, end);
        }
    }
}