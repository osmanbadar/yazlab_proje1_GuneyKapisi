using UnityEngine;
using GuneyKapisi.NPC;
namespace GuneyKapisi.Testing
{
    
    public class ParserSelfTest : MonoBehaviour
    {
        [ContextMenu("Parser testlerini çalıştır")]
        private void Run()
        {
            var parser = new NpcResponseParser();
            var cases = new (string name, string raw, bool ok, NpcAction action)[]
            {
 ("Geçerli",
 @"{""reply"":""Geç."",""action"":""open_door"",""mood"":""sakin""}",
 true, NpcAction.OpenDoor),
 ("Önünde metin", @"Tabii! {""reply"":""Hayır."",""action"":""none""}",
 true, NpcAction.None),
 ("Büyük harf", @"{""reply"":""Defol!"",""action"":""ATTACK""}",
 true, NpcAction.Attack),
 ("Bilinmeyen eylem", @"{""reply"":""Hmm."",""action"":""dance""}",
 true, NpcAction.None),
 ("Yarım JSON", @"{""reply"":""Yarım kal", false, NpcAction.None),
 ("reply yok", @"{""action"":""open_door""}", false, NpcAction.None),
 ("JSON değil", "Merhaba yolcu", false, NpcAction.None),
 ("Boş", "", false, NpcAction.None)
            };
            int passed = 0;
            foreach (var c in cases)
            {
                bool ok = parser.TryParse(c.raw, out NpcResponse r);
                bool pass = ok == c.ok && (!ok || r.Action == c.action);
                if (pass) passed++;
                Debug.Log($"[ParserTest] {(pass ? "GEÇTİ" : "KALDI")} - {c.name}: " +
                $"ok={ok}, action={(ok ? r.Action.ToString() : "-")}");
            }
            Debug.Log($"[ParserTest] Sonuç: {passed}/{cases.Length}");
        }
    }
}
