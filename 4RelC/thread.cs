using System;
using System.Runtime.InteropServices;
using System.Threading;

public class ThreadHelper : IThread
{
    private static object lockObject = new object();
    private static volatile int globalResponse = 0;

    private const int COUNT_KEY = 0;
    private const int YES = 1;
    private const int NO = 2;

    public void BusyWait(int ms)
    {
        Thread.Sleep(ms);
    }

    public void StartThread()
    {
        Thread thread = new Thread(WaitForKey);
        thread.Priority = ThreadPriority.Highest;
        thread.Start();
    }

    public int CheckThreadResult()
    {
        int res;
        lock (lockObject)
        {
            res = globalResponse;
        }
        return res;
    }

    private static void WaitForKey()
    {
        char resp;
        int respI = NO;

        var termInfo = new termios();
        tcgetattr(0, ref termInfo);
        termInfo.c_lflag &= ~ICANON;
        termInfo.c_cc[VMIN] = 1;
        termInfo.c_cc[VTIME] = 0;
        tcsetattr(0, TCSANOW, ref termInfo);

        piHiPri(10);
        resp = (char)Console.Read();
        if (resp == 'y' || resp == 'Y')
        {
            respI = YES;
        }

        lock (lockObject)
        {
            globalResponse = respI;
        }

        termInfo.c_lflag |= ICANON;
        termInfo.c_cc[VMIN] = 0;
        termInfo.c_cc[VTIME] = 0;
        tcsetattr(0, TCSANOW, ref termInfo);

        Console.WriteLine();
    }

    private static int piHiPri(int pri)
    {
        // Not applicable in .NET, use Thread.Priority instead
        return 0;
    }

    private static void tcgetattr(int fd, ref termios termios)
    {
        // Not applicable in .NET
    }

    private static void tcsetattr(int fd, int optional_actions, ref termios termios)
    {
        // Not applicable in .NET
    }

    private const int ICANON = 2;
    private const int VMIN = 16;
    private const int VTIME = 17;

    [StructLayout(LayoutKind.Sequential)]
    private struct termios
    {
        public uint c_iflag;
        public uint c_oflag;
        public uint c_cflag;
        public uint c_lflag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] c_cc;
        public uint c_ispeed;
        public uint c_ospeed;
    }
}
