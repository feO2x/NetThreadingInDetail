// ReSharper disable ArgumentsStyleLiteral
namespace ThreadPoolBehaviorOnSleep
{
    internal class Program
    {
        private static void Main()
        {
            var performanceTest = new ThreadPoolPerformance();
            performanceTest.RunWorkItemsOnThreadPool(shouldThreadsSleep: false)
                           .RunWorkItemsOnThreadPool(shouldThreadsSleep: true);
        }
    }
}