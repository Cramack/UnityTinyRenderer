using System;
using UnityEngine;

public static class RenderingHelper
{
    public static Vector3 BaryCentric(Vector2Int v0, Vector2Int v1, Vector2Int v2, Vector2Int p)
    {
        var a = new Vector3(v2.x - v0.x, v1.x - v0.x, v0.x - p.x);
        var b = new Vector3(v2.y - v0.y, v1.y - v0.y, v0.y - p.y);
        //calculate cross product
        var cross = Vector3.Cross(a, b);

        //triangle is degenerate
        if (Mathf.Abs(cross.z) < 1)
        {
            return new Vector3(-1, 1, 1);
        }

        return new Vector3(1 - (cross.x + cross.y) / cross.z, cross.x / cross.z, cross.y / cross.z);
    }

    public static void FillArrayV1<T>(T[] arr, T value)
    {
        int len = arr.Length;
        for (int i = 0; i < len; i++)
        {
            arr[i] = value;
        }
    }


    /// <summary>
    /// https://stackoverflow.com/questions/5943850/fastest-way-to-fill-an-array-with-a-single-value
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void FillArrayV2<T>(T[] arr, T value)
    {
        int length = arr.Length;
        if (length == 0)
        {
            return;
        }

        arr[0] = value;

        int arrayHalfLen = length / 2;
        int copyLength;
        for (copyLength = 1; copyLength <= arrayHalfLen; copyLength <<= 1)
        {
            Array.Copy(arr, 0, arr, copyLength, copyLength);
        }

        Array.Copy(arr, 0, arr, copyLength, length - copyLength);
    }
}