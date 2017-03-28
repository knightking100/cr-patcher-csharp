using Ionic.Zip;
using System;
using System.IO;

namespace cr_patcher_csharp
{
    public partial class Patcher
    {
        public static void PatchIPA(string ipa)
        {
            ExtractBinary(ipa);
            Console.WriteLine("Start patching...");
            Patch($"Payload\\Clash Royale.app\\Clash Royale", GetKey(ipa));
            
            Console.WriteLine("Patched completed! Saving ipa");
            AddBinary(ipa);
            Console.WriteLine("Saved ipa");
        }
        public static void ExtractBinary(string ipaName)
        {
            if (Directory.Exists("Payload"))
                Delete("Payload");
            using (ZipFile ipa = new ZipFile(ipaName))
            {
                if (ipa.ContainsEntry("Payload/Clash Royale.app/Clash Royale"))
                {
                    Console.Write("Extracting binary...");
                    ZipEntry binary = ipa["Payload/Clash Royale.app/Clash Royale"];
                    binary.Extract();
                    Console.WriteLine("DONE");
                }
                else
                    Console.WriteLine("Binary wasn't found");
            }
        }
        public static void AddBinary(string ipaName)
        {
            ZipFile ipa = new ZipFile(ipaName);

            if (ipa.ContainsEntry("Payload/Clash Royale.app/Clash Royale"))
            {
                ipa.RemoveEntry("Payload/Clash Royale.app/Clash Royale");
            }
            ipa.AddFile("Payload\\Clash Royale.app\\Clash Royale", "Payload/Clash Royale.app");
            
            ipa.Save();
        }
    }
}
