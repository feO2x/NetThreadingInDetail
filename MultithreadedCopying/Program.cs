using System;
using System.Diagnostics;

namespace MultithreadedCopying
{
    internal class Program
    {
        private const int SmallArraySize = 100000000; // 100 Million ints
        private const int LargeArraySize = 150000000; // 150 Million ints

        private static void Main()
        {
            var largeArray = InitializeLargeArray();
            var evenLargerArray = new int[LargeArraySize];

            var stopwatch = Stopwatch.StartNew();
            //CopySingleThreaded(largeArray, evenLargerArray);
            new LockFreeMultiThreadedCopying(largeArray, evenLargerArray).Run();

            Console.WriteLine($"Copying {largeArray.Length:N0} items took {stopwatch.Elapsed.TotalMilliseconds:N}ms");
        }

        private static int[] InitializeLargeArray()
        {
            var array = new int[SmallArraySize];
            var random = new Random(19943832);
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = random.Next();
            }

            return array;
        }

        private static void CopySingleThreaded(int[] oldArray, int[] newArray)
        {
            for (var i = 0; i < oldArray.Length; i++)
            {
                newArray[i] = oldArray[i];
            }
        }
    }
}