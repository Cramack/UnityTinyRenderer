using System;
using UnityEngine;

public static class RenderingHelper
{
    /// <summary>
    /// 重心坐标系;
    /// 其实就是种插值. 可以简单的通过二维的线性插值来理解.
    /// 在degenerate的情况下, 返回的是(-1,-1,-1)
    /// </summary>
    public static Vector3 BaryCentric(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int p)
    {
        var a1 = b.x - a.x;
        var b1 = c.x - a.x;
        var c1 = p.x - a.x;
        var a2 = b.y - a.y;
        var b2 = c.y - a.y;
        var c2 = p.y - a.y;

        var d = a1 * b2 - a2 * b1;
        if (d == 0) // degenerate triangle 
        {
            return new Vector3(1, 0, 0);
        }

        var u = (float)(b2 * c1 - b1 * c2) / d;
        var v = (float)(a1 * c2 - a2 * c1) / d;
        var w = 1 - u - v;
        return new Vector3(u, v, w);
    }

    /// <summary>
    /// https://stackoverflow.com/questions/5943850/fastest-way-to-fill-an-array-with-a-single-value
    /// 主要是能用类似block的函数来填充数组, 速度会快很多. 利用了类似于指数的优化. 
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void FillArray<T>(T[] arr, T value)
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