using UnityEngine;
using UnityEngine.InputSystem;
using GuneyKapisi.UI;
namespace GuneyKapisi.Testing
{
    // Geçici: T tuşu ile diyaloğu açar. Hafta 2'de GuardInteractionZone gelecek.
    public class DebugDialogueOpener : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
                dialogueUI.Open();
        }
    }
}