using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace cr_patcher_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            Console.WriteLine($"Enter APK/IPA filename in this folder");
            string name = Console.ReadLine();
            Do(name);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
        static void Do(string path)
        {
            if(!File.Exists(path))
            {
                Console.WriteLine($"File '{path}' doesn't exist");
                return;
            }
            Console.Title = $"{Title} - {path}";

            Console.WriteLine("Starting operation...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            Patcher.Start(path);
            
            sw.Stop();
            Console.WriteLine($"Finished operation in {sw.Elapsed.TotalMilliseconds}ms");
            
        }

        private static bool CheckRequirement()
        {
            Console.WriteLine("Checking requirements...");
            
            if(!File.Exists("Tools\\signapk.jar"))
            {
                Console.WriteLine("signapk.jar.. FAILED");
                return false;
            }
            Console.WriteLine(" --> signapk.jar...OK");
            
            if (!File.Exists("Tools\\zipalign.exe"))
            {
                Console.WriteLine("zipalign.exe...FAILED");
                return false;
            }
            Console.WriteLine(" --> zipalign.exe...OK");
            
            if(!JavaInstalled())
            {
                Console.WriteLine("Java Not installed yet");
                return false;
            }
            Console.WriteLine(" --> Java...INSTALLED");
            return true;
        }
        private static bool JavaInstalled()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall") ??
                  Registry.LocalMachine.OpenSubKey(
                      @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key == null)
                return false;

            return key.GetSubKeyNames()
                .Select(keyName => key.OpenSubKey(keyName))
                .Select(subkey => subkey.GetValue("DisplayName") as string)
                .Any(displayName => displayName != null && displayName.Contains("Java"));
        
        }
        private static void ShowWelcomeMessage()
        {
            Console.Title = Title;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            WriteCenter($"--- {Title} by {Author} ---");
            WriteCenter($"--- Latest source code can be found at {GitHub} ---");
            WriteCenter(StartMessage);
            Console.ResetColor();
        }
        private static void WriteCenter(string str)
        {
            Console.WriteLine(str);
            
        }
        /* Some information
         */
        static string Title = "Clash Royale Patcher Sharp v1.1";//Should be assembly version but i'm lazy :P
        static string Author = "knightking10";
        static string GitHub = "https://github.com/knightking100/cr-patcher-csharp";
        static string StartMessage = $"cr-patcher-sharp is starting...";
    }
}
