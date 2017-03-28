using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cr_patcher_csharp
{
    public partial class Patcher
    {
        public static void Start(string path)
        {
            if (Path.GetExtension(path) == ".ipa")
            {
                PatchIPA(path);
            }
            else
                PatchAPK(path);
        }
        public static string ReadVersion(string apkname)
        {
            return Path.GetFileNameWithoutExtension(apkname).Replace($"{_config.PackageName}-", "");
        }
        public static string GetKey(string apkname)
        {
            if (_config.versions.TryGetValue(ReadVersion(apkname), out string value))
            {
                return value;
            }
            
            return "72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e";//TODO : update

        }
        private static void Delete(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        private static void Patch(string path, string key)
        {
            Console.WriteLine($"Patching {path}...");

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
                
            }
            Console.WriteLine($"Patched {path} successfully!");
        }
    }
}
