using System;
using System.Threading;

namespace MultithreadingWithoutSynchronization
{
    internal class Program
    {
        private static bool _isComplete;

        private static void Main()
        {
            _isComplete = false;
            var thread = new Thread(Spin);
            thread.Start();

            Thread.Sleep(2500);

            _isComplete = true;
            thread.Join();
            Console.WriteLine("Other thread completed spinning");
        }

        private static void Spin()
        {
            var isActive = false;
            while (_isComplete == false)
                isActive = !isActive;
        }
    }
}