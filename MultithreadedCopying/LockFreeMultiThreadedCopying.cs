using System;
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
            var tasks = Enumerable.Range(0, Environment.ProcessorCount)
                                  .Select(i => Task.Run(new Action(CopyFromOldToNew)))
                                  .ToArray();
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