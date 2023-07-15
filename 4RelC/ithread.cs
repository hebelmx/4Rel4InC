using System;

public interface IThread
{
    void BusyWait(int ms);
    void StartThread();
    int CheckThreadResult();
}

public static class ThreadExtensions
{
    public static void BusyWait(this IThread thread, int ms)
    {
        thread.BusyWait(ms);
    }

    public static void StartThread(this IThread thread)
    {
        thread.StartThread();
    }

    public static int CheckThreadResult(this IThread thread)
    {
        return thread.CheckThreadResult();
    }
}
