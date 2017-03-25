using System;
using System.Diagnostics;

namespace cr_patcher_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("cr-patcher-csharp");
            if (args.Length == 0 || args[0] ==  null)
            {
                Console.WriteLine("Enter apk name");
                string apk = Console.ReadLine();
                Do(apk);
            }
            else
            {
                Do(args[0]);
            }
            
        }
        static void Do(string apk)
        {
            Console.Title = $"cr-patcher-csharp [{apk}]";

            Console.WriteLine("Starting operation...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Patcher.ExtractLibg(apk);

            Patcher.Patch(apk);
            Apktool.SignAPK(apk);
            Apktool.ZipalignAPK(apk);

            sw.Stop();
            Console.WriteLine($"Finished operation in {sw.Elapsed.TotalMilliseconds}ms");
            Console.ReadLine();
        }
    }
}
