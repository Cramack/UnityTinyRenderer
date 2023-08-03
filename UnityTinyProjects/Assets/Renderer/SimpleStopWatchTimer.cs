using System;
using System.Diagnostics;
using UnityEngine.Pool;

/// <summary>
/// 一个简单的计时器，用于测试性能
/// </summary>
class SimpleStopWatchTimer : IDisposable
{
    public SimpleStopWatchTimer()
    {
        this.m_sw = new Stopwatch();
    }

    public static SimpleStopWatchTimer Get(string name)
    {
        SimpleStopWatchTimer st;
        GenericPool<SimpleStopWatchTimer>.Get(out st);
        st.m_sw.Reset();
        st.m_name = name;
        st.m_sw.Start();
        return st;
    }

    public static void Release(SimpleStopWatchTimer st)
    {
        GenericPool<SimpleStopWatchTimer>.Release(st);
    }

    public void Dispose()
    {
        this.m_sw.Stop();
        UnityEngine.Debug.Log(string.Format("{0} run {1} secs", this.m_name, this.m_sw.Elapsed.TotalSeconds));
        Release(this);
    }

    string m_name;
    private Stopwatch m_sw;
}