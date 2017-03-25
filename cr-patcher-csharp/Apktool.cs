using System;
using System.Diagnostics;
using System.IO;
namespace cr_patcher_csharp
{
    public class Apktool
    {
        public static void SignAPK(string apkname)
        {
            Console.WriteLine($"Start signing {apkname}");
            var sign = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $@"/c Tools\signapk.jar Tools\certificate.pem Tools\key.pk8 {apkname} {Path.GetFileNameWithoutExtension(apkname)}-signed.apk",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var p = Process.Start(sign);
            p.WaitForExit();
            Console.WriteLine($"Signed {apkname}");
        }
        public static void ZipalignAPK(string apkname)
        {
            Console.WriteLine($"Start zipaligning {apkname}");
            var sign = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $@"/c Tools\zipalign -f -v 4 {Path.GetFileNameWithoutExtension(apkname)}-signed.apk {Path.GetFileNameWithoutExtension(apkname)}-completed.apk",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var p = Process.Start(sign);
            p.Exited += (sender, e)
                =>
            {
                Console.WriteLine("lol");
            };
            Console.WriteLine($"Zipaligned {apkname}");
            
        }
    }
}
