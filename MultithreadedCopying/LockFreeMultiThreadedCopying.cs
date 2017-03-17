using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace MultithreadedCopying
{
    public sealed class LockFreeMultiThreadedCopying
    {
        private readonly int[] _newArray;
        private readonly int[] _oldArray;
        private int _currentIndex = -1;

        public LockFreeMultiThreadedCopying(int[] oldArray, int[] newArray)
        {
            newArray.Length.MustBeGreaterThan(oldArray.Length);

            _oldArray = oldArray;
            _newArray = newArray;
        }

        public void Run()
        {
            var numberOfProcessors = Environment.ProcessorCount;
            var tasks = new Task[numberOfProcessors];
            var copyFromOldToNewDelegate = new Action(CopyFromOldToNew);
            for (var i = 0; i < numberOfProcessors; i++)
            {
                tasks[i] = Task.Run(copyFromOldToNewDelegate);
            }
            
            Task.WaitAll(tasks);
        }

        private void CopyFromOldToNew()
        {
            while (true)
            {
                var i = Interlocked.Increment(ref _currentIndex);
                if (i >= _oldArray.Length)
                    return;

                _newArray[i] = _oldArray[i];
            }
        }
    }
}