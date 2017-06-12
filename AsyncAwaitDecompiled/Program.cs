using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AsyncAwaitDecompiled
{
    public static class Program
    {
        private static void Main()
        {
            var readers = InitializeReaders();
            var numberOfWords = AsyncDecompiled.CalculateTotalNumberOfWordsAsync(readers).Result;
            readers.DisposeAll();
            Console.WriteLine($"The {readers.Length} streams contain a total number of {numberOfWords} words.");
        }

        private static TextReader[] InitializeReaders()
        {
            return new TextReader[]
                   {
                       InitializeStreamReader("TextInFile.txt"),
                       InitializeStreamReader("SomeHierarchicalText.json")
                   };
        }

        private static StreamReader InitializeStreamReader(string path)
        {
            return new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous));
        }

        public static async Task<int> CalculateTotalNumberOfWordsAsync(IEnumerable<TextReader> readers)
        {
            var totalNumberOfWords = 0;
            foreach (var reader in readers)
            {
                var content = await reader.ReadToEndAsync();
                totalNumberOfWords += content.CalculateNumberOfWords();
            }

            return totalNumberOfWords;
        }
    }
}