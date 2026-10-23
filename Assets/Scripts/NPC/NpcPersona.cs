using UnityEngine;
namespace GuneyKapisi.NPC
{
    [CreateAssetMenu(fileName = "NpcPersona", menuName = "GuneyKapisi/NPC Persona")]
    public class NpcPersona : ScriptableObject
    {
        public string npcName = "Gorvan";
        [TextArea(12, 40)]
        public string systemPrompt;
    }
}