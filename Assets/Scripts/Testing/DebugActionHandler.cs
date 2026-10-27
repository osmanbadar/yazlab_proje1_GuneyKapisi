using UnityEngine;
using GuneyKapisi.NPC;
namespace GuneyKapisi.Testing
{
    // Geçici: eylemi yalnızca Console'a yazar. Hafta 2'de DoorActionHandler gelecek.
    public class DebugActionHandler : MonoBehaviour, INpcActionHandler
    {
        public void Handle(NpcAction action) => Debug.Log($"[Eylem] {action}");
    }
}
