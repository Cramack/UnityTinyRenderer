using System;
using UnityEngine;

public static class RenderingHelper
{
    public static Vector3 BaryCentric(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int p)
    {
        var a1 = b.x - a.x;
        var b1 = c.x - a.x;
        var c1 = p.x - a.x;
        var a2 = b.y - a.y;
        var b2 = c.y - a.y;
        var c2 = p.y - a.y;

        var d = a1 * b2 - a2 * b1;
        if (d == 0)
        {
            return new Vector3(-1, -1, -1);
        }

        var u = (float)(b2 * c1 - b1 * c2) / d;
        var v = (float)(a1 * c2 - a2 * c1) / d;
        var w = 1 - u - v;
        return new Vector3(u, v, w);
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