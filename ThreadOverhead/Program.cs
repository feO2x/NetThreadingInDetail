using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadOverhead
{
    public static class Program
    {
        private static readonly ManualResetEventSlim WakeThreadsEvent = new ManualResetEventSlim(false);

        public static void Main()
        {
            var numberOfThreads = 0;
            try
            {
                while (true)
                {
                    var newThread = new Thread(KeepThreadSleepingInMemory);
                    newThread.Start();
                    ++numberOfThreads;
                    Console.WriteLine($"Number of threads: {numberOfThreads}\tAllocated Memory: {Process.GetCurrentProcess().PrivateMemorySize64.InKiloBytes()}");
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine($"Out of memory after {numberOfThreads} threads.");
                WakeThreadsEvent.Set();
                WakeThreadsEvent.Dispose();
            }

            Console.WriteLine("Press ENTER to quit");
            Console.ReadLine();
        }

        private static void KeepThreadSleepingInMemory()
        {
            WakeThreadsEvent.Wait();
        }

        private static string InKiloBytes(this long numberOfBytes)
        {
            return $"{numberOfBytes / 1024:N0} KB";
        }
    }
}