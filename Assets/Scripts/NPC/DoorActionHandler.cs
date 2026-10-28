using UnityEngine;
using GuneyKapisi.Core;
namespace GuneyKapisi.NPC
{
    // Gorvan'ın kararını oyun dünyasına uygular (DebugActionHandler'ın gerçeği).
    public class DoorActionHandler : MonoBehaviour, INpcActionHandler
    {
        [SerializeField] private Door door;
        public void Handle(NpcAction action)
        {
            Debug.Log($"[Eylem] {action}");
            switch (action)
            {
                case NpcAction.OpenDoor:
                    door.Open();
                    break;
                case NpcAction.Attack:
                    GameEvents.RaisePlayerDefeated(); // GameManager -> Lost
                    break;
                    // None: konuşma sürer. EndConversation: GuardNPC paneli kapatır.
            }
        }
    }
}
