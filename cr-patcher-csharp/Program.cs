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
            string CrayCray = "cr-patcher-csharp";
            string titl = $"CrayCray Modded APK Downloader";
            int length = (Console.WindowWidth - CrayCray.Length) / 2;
            string spaces = new string(' ',length);
            Console.Title = $"{spaces}CrayCray Modded APK Downloader{spaces}";
            Console.SetCursorPosition((Console.WindowWidth - CrayCray.Length) / 2, Console.CursorTop);
            WriteLineCenter("CrayCray MODDED APK Downloader ALPHA");
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            if (CheckRequirement())
            {
                if (args.Length == 0 || args[0] == null)
                {
                    WriteLineCenter("Enter apk name in this folder");
                    string apk = Console.ReadLine();
                    Do(apk);
                }
                else
                {
                    Do(args[0]);
                }

            }
            Console.ReadLine();
        }
        static void Do(string apk)
        {
            Console.Title = $"CrayCray MODDED APK Downloader ALPHA [Downloading {apk}...]";

            WriteLineCenter("Starting operation...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Patcher.ExtractLibg(apk);

            Patcher.Patch(apk);
            Apktool.SignAPK(apk);
            Apktool.ZipalignAPK(apk);

            sw.Stop();
            WriteLineCenter($"Finished operation in {sw.Elapsed.TotalMilliseconds}ms");
            File.Delete($"{Path.GetFileNameWithoutExtension(apk)}-signed.apk");
            
        }
        private static bool CheckRequirement()
        {
            WriteLineCenter("Checking requirements...");
            
            if(!File.Exists("Tools\\signapk.jar"))
            {
                WriteLineCenter("signapk.jar.. FAILED");
                return false;
            }
            WriteLineCenter(" --> signapk.jar...OK");
            
            if (!File.Exists("Tools\\zipalign.exe"))
            {
                WriteLineCenter("zipalign.exe...FAILED");
                return false;
            }
            WriteLineCenter(" --> zipalign.exe...OK");
            
            if(!JavaInstalled())
            {
                WriteLineCenter("Java Not installed yet");
                return false;
            }
            WriteLineCenter(" --> Java...INSTALLED");
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
        private static void WriteLineCenter(object xD)
        {
            Console.SetCursorPosition((Console.WindowWidth - xD.ToString().Length) / 2, Console.CursorTop);
            Console.WriteLine(xD);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }
        private static void WriteCenter(object xD)
        {
            Console.SetCursorPosition((Console.WindowWidth - xD.ToString().Length) / 2, Console.CursorTop);
            Console.Write(xD);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }
    }
}
