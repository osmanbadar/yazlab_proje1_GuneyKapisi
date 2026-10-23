using Newtonsoft.Json.Linq;
namespace GuneyKapisi.NPC
{
    
    public static class NpcResponseSchema
    {
        public static JObject Create() => JObject.Parse(@"
 {
 ""type"": ""object"",
 ""properties"": {
 ""reply"": { ""type"": ""string"" },
 ""action"": { ""type"": ""string"",
 ""enum"": [""none"", ""open_door"", ""attack"",
 ""end_conversation""] },
 ""mood"": { ""type"": ""string"" }
 },
 ""required"": [""reply"", ""action"", ""mood""]
 }");
    }
}