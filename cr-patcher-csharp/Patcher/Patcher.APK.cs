using System;
using System.IO;
using Ionic.Zip;

namespace cr_patcher_csharp
{
    public partial class Patcher
    {
        private static Config _config { get; set; } = Config.Read("config.json");
        
        public static void PatchAPK(string apkname)
        {
            ExtractLibg(apkname);
            Console.WriteLine("Start patching...");
            Patch("lib\\x86\\libg.so", GetKey(apkname));
            Patch("lib\\armeabi-v7a\\libg.so", GetKey(apkname));

            Console.WriteLine("Patched completed! Saving apk");
            Addlibg(apkname);
            Console.WriteLine("Saved apk.");
            Apktool.SignAPK(apkname);
            Apktool.ZipalignAPK(apkname);
        }
        private static void Addlibg(string apkname)
        {
            ZipFile file = new ZipFile(apkname);

            if (file.ContainsEntry("lib/x86/libg.so"))
            {
                file.RemoveEntry("lib/x86/libg.so");
            }
            file.AddFile("lib\\x86\\libg.so", "lib/x86");
            if (file.ContainsEntry("lib/armeabi-v7a/libg.so"))
            {
                file.RemoveEntry("lib/armeabi-v7a/libg.so");
            }
            file.AddFile("lib\\armeabi-v7a\\libg.so", "lib/armeabi-v7a");
            file.Save();
            
        }
        public static void ExtractLibg(string apkname)
        {
            if (Directory.Exists("lib"))
                Delete("lib");
            using (ZipFile apk = new ZipFile(apkname))
            {
                if (apk.ContainsEntry("lib/x86/libg.so") && apk.ContainsEntry("lib/armeabi-v7a/libg.so"))
                {
                    Console.Write("Extracting libg.so...");
                    ZipEntry x86lib = apk["lib/x86/libg.so"];
                    ZipEntry armLib = apk["lib/armeabi-v7a/libg.so"];
                    x86lib.Extract();
                    armLib.Extract();
                    Console.WriteLine("DONE");
                }
                else
                    Console.WriteLine("libg.so wasn't found");
            }

        }
        
    }
}
