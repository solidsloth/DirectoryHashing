using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"S:\marke\Desktop\Hash Me";
            int testCount = 100000;

            long bitConverter = RunTest(new BitConverterHasher(), path, testCount);
            long stringBuilderConverter = RunTest(new Hasher(), path, testCount);

            Console.WriteLine($"Bit Converter:      {bitConverter/1000}s");
            Console.WriteLine($"Builder Converter:  {stringBuilderConverter/1000}s");

            Console.ReadLine();
        }

        public static long RunTest(IHasher hasher, string path, int testCount)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < testCount; i++)
            {
                using (var md5 = MD5.Create())
                {
                    string hash = hasher.GetDirectoryHash(path, md5)
                        .GetAwaiter()
                        .GetResult();
                }
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
