using System;
using System.Collections.Generic;

namespace AsyncAwaitDecompiled
{
    public static class ExtensionMethods
    {
        public static int CalculateNumberOfWords(this string content)
        {
            var numberOfWords = 0;
            var isBeforeWord = true;
            foreach (var character in content)
            {
                if (char.IsWhiteSpace(character))
                {
                    isBeforeWord = true;
                    continue;
                }

                if (isBeforeWord)
                {
                    isBeforeWord = false;
                    ++numberOfWords;
                }
            }
            return numberOfWords;
        }

        public static void DisposeAll<T>(this IEnumerable<T> disposables) where T : IDisposable
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}