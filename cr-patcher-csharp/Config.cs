using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cr_patcher_csharp
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Config
    {
        [JsonProperty("package")]
        public string PackageName { get; set; } = "com.supercell.clashroyale";
        [JsonProperty("key")]
        public string Key { get; set; } = "72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e";
        [JsonProperty("keyStore")]
        public Dictionary<string, string> versions { get; set; } = new Dictionary<string, string>()
        {
            { "1.8.0" , "9e6657f2b419c237f6aeef37088690a642010586a7bd9018a15652bab8370f4f"},
            { "1.7.0", "0f9fff6d583023c5c739c053581c994dbe37789900ffda312fc97edfd091945f"},
            {"1.6.0","e330c7916ae0a66f3a90eae97a863ee00ac1dcad058877b1eecfc8fe91c93532" },
            { "1.5.0", "bbdba8653396d1df84efaea923ecd150d15eb526a46a6c39b53dac974fff3829"},
            { "1.4.1" , "9bc23206948f104820e347ed47fa92256ca843b72aec503a0982889cd6a7eb38"},
            { "1.2.3" , "ba105f0d3a099414d154046f41d80cf122b49902eab03b78a912f3c66dba2c39"}
            
        };
      
        public static void Create(string path)
        {
            File.WriteAllText(path,JsonConvert.SerializeObject(new Config(),Formatting.Indented));
        }
        public static Config Read(string path)
        {
            if(!File.Exists(path))
            {
                Console.WriteLine("Config not found.Creating one...");
                Create(path);
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
    }
}
