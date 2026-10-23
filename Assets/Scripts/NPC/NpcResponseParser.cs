using System;
using Newtonsoft.Json.Linq;
namespace GuneyKapisi.NPC
{
    public class NpcResponseParser : IResponseParser<NpcResponse>
    {
        public bool TryParse(string raw, out NpcResponse result)
        {
            result = null;
            string json = ExtractJsonObject(raw);
            if (json == null) return false;
            try
            {
                JObject obj = JObject.Parse(json);
                string reply = ((string)obj["reply"])?.Trim();
                if (string.IsNullOrEmpty(reply)) return false; 
                NpcAction action = ParseAction((string)obj["action"]);
                string mood = (string)obj["mood"] ?? "";
                result = new NpcResponse(reply, action, mood);
                return true;
            }
            catch (Exception) 
            {
                return false;
            }
        }
        
        private static string ExtractJsonObject(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            int start = raw.IndexOf('{');
            int end = raw.LastIndexOf('}');
            return (start >= 0 && end > start) ? raw.Substring(start, end - start + 1) : null;
        }
        
        private static NpcAction ParseAction(string value)
        {
            switch (value?.Trim().ToLowerInvariant())
            {
                case "open_door": return NpcAction.OpenDoor;
                case "attack": return NpcAction.Attack;
                case "end_conversation": return NpcAction.EndConversation;
                default: return NpcAction.None;
            }
        }
    }
}

