using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadOverhead
{
    public static class Program
    {
        private static readonly ManualResetEvent WakeThreadsEvent = new ManualResetEvent(false);

        public static void Main()
        {
            var numberOfThreads = 0;
            try
            {
                while (true)
                {
                    var newThread = new Thread(WaitUntilEventSignals);
                    newThread.Start();
                    Console.WriteLine($"Number of threads: {++numberOfThreads}\tAllocated Memory: {Process.GetCurrentProcess().PrivateMemorySize64.InMegabytes()}");
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine($"Out of memory after {numberOfThreads} threads.");
                WakeThreadsEvent.Set();
                WakeThreadsEvent.Close();
            }
        }

        private static void WaitUntilEventSignals()
        {
            WakeThreadsEvent.WaitOne();
        }

        private static string InMegabytes(this long numberOfBytes)
        {
            return $"{numberOfBytes / 1024} KB";
        }
    }
}