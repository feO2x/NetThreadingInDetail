using System;
using System.Diagnostics;
using System.Threading;
using Light.GuardClauses;

namespace ThreadPoolBehaviorOnSleep
{
    public sealed class ThreadPoolPerformance
    {
        private readonly int _amountOfWork;
        private readonly int _numberOfItems;
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _numberOfItemsProcessed;
        private int _maximumNumberOfThreads;
        private int _numberOfCurrentThreads;
        private bool _shouldThreadsSleep;

        public ThreadPoolPerformance(int numberOfItems = 500, int amountOfWork = 500)
        {
            numberOfItems.MustNotBeLessThan(1);
            amountOfWork.MustNotBeLessThan(20);

            _numberOfItems = numberOfItems;
            _amountOfWork = amountOfWork;
        }

        public ThreadPoolPerformance RunWorkItemsOnThreadPool(bool shouldThreadsSleep)
        {
            _maximumNumberOfThreads = _numberOfCurrentThreads = _numberOfItemsProcessed = 0;
            _shouldThreadsSleep = shouldThreadsSleep;

            _stopwatch.Restart();
            for (var i = 0; i < _numberOfItems; ++i)
            {
                ThreadPool.QueueUserWorkItem(PerformWorkOnThreadPool);
            }
            Console.WriteLine($"{_numberOfItems} items were queued on the thread pool.");

            _resetEvent.WaitOne();

            Console.WriteLine($"Performed work in {_stopwatch.Elapsed.TotalSeconds:N2}s on a maximum of {_maximumNumberOfThreads} threads.");
            return this;
        }

        private void PerformWorkOnThreadPool(object state)
        {
            // Check how many threads are active
            var numberOfCurrentThreads = Interlocked.Increment(ref _numberOfCurrentThreads);
            InterlockedMaximum(ref _maximumNumberOfThreads, numberOfCurrentThreads);

            // Do some actual work (or go to sleep)
            if (_shouldThreadsSleep == false)
                // ReSharper disable once EmptyForStatement
                // ReSharper disable once EmptyEmbeddedStatement
                for (long targetNumberOfTicks = Environment.TickCount + _amountOfWork; Environment.TickCount < targetNumberOfTicks;) ;
            else
                Thread.Sleep(_amountOfWork);

            // Decrement current threads and finish if necessray
            Interlocked.Decrement(ref _numberOfCurrentThreads);
            var numberOfItemsProcessed = Interlocked.Increment(ref _numberOfItemsProcessed);
            if (numberOfItemsProcessed == _numberOfItems)
                _resetEvent.Set();
        }

        private static void InterlockedMaximum(ref int target, int value)
        {
            int temporaryValue;
            var readValueOfTarget = target;
            do
            {
                temporaryValue = readValueOfTarget;
                readValueOfTarget = Interlocked.CompareExchange(ref target, Math.Max(temporaryValue, value), temporaryValue);
            } while (temporaryValue != readValueOfTarget);
        }
    }
}