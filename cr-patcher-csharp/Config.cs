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
            { "1.8.0" , "9e6657f2b419c237f6aeef37088690a642010586a7bd9018a15652bab8370f4f"}
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
