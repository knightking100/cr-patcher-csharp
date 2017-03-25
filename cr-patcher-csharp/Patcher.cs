using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;

namespace cr_patcher_csharp
{
    public class Patcher
    {
        private static Config _config { get; set; } = Config.Read("config.json");
        
        public static void Patch(string apkname)
        {
            Console.WriteLine("Start patching...");
            PatchLibg("lib\\x86\\libg.so", GetKey(apkname));
            PatchLibg("lib\\armeabi-v7a\\libg.so", GetKey(apkname));

            Console.WriteLine("Patched completed! Saving apk");
            Addlibg(apkname);
            Console.WriteLine("Saved apk.");
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
                    x86lib.Extract(Environment.CurrentDirectory);
                    armLib.Extract(Environment.CurrentDirectory);
                    Console.WriteLine("DONE");
                }
                else
                    Console.WriteLine("libg.so wasn't found");
            }

        }
        public static string ReadVersion(string apkname)
        {
            return Path.GetFileNameWithoutExtension(apkname).Replace($"{_config.PackageName}-","");
        }
        public static string GetKey(string apkname)
        {
            if(_config.versions.TryGetValue(ReadVersion(apkname),out string value))
            {
                return value;
            }
            return //"72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e";
            "s";
        }
        private static void Delete(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        private static void PatchLibg(string path, string key)
        {
            Console.WriteLine($"Patching {path}...");
            //var key = GetKey(path);
            byte[] fileBytes = File.ReadAllBytes(path);
            byte[] searchPattern = key.ToByteArray();
            byte[] replacePattern = _config.Key.ToByteArray();
            IEnumerable<int> positions = fileBytes.FindPattern(searchPattern);
            if (positions.Count() == 0)
            {
                Console.WriteLine("Key wasn't found in key store.");
                return;
            }
            
            foreach (int pos in positions)
            {
                Console.WriteLine("Key offset: 0x" + pos.ToString("X8"));
                using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Open, FileAccess.Write)))
                {
                    bw.BaseStream.Seek(pos, SeekOrigin.Begin);
                    bw.Write(replacePattern);
                }
                Console.WriteLine($"Patched {path} successfully!");
            }
        }
    }
}
