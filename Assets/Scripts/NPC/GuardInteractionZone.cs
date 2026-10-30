using UnityEngine;
using UnityEngine.InputSystem;
using GuneyKapisi.Core;
using GuneyKapisi.UI;
namespace GuneyKapisi.NPC
{
    // Bekçinin etrafındaki görünmez alan. Oyuncu içindeyken E -> diyalog.
    [RequireComponent(typeof(Collider))]
    public class GuardInteractionZone : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private GameObject talkPrompt; // "Konuşmak için E"
        private bool _playerInside;
        private void Reset() => GetComponent<Collider>().isTrigger = true;
        private void Awake() => talkPrompt.SetActive(false);
        private void OnEnable()
        {
            GameEvents.OnDialogueToggled += HandleDialogueToggled;
            gameManager.StateChanged += HandleStateChanged;
        }
        private void OnDisable()
        {
            GameEvents.OnDialogueToggled -= HandleDialogueToggled;
            gameManager.StateChanged -= HandleStateChanged;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInside = true;
            GameEvents.RaiseGateReached(); // GameManager: Camp -> Gate
            RefreshPrompt();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInside = false;
            RefreshPrompt();
        }
        private void Update()
        {
            if (!CanTalk() || Keyboard.current == null) return;
            if (Keyboard.current.eKey.wasPressedThisFrame)
                dialogueUI.Open();
        }
        private bool CanTalk() =>
        _playerInside && !dialogueUI.IsOpen && gameManager.State == GameState.Gate;
        private void HandleDialogueToggled(bool _) => RefreshPrompt();
        private void HandleStateChanged(GameState _) => RefreshPrompt();
        private void RefreshPrompt() => talkPrompt.SetActive(CanTalk());
    }
}