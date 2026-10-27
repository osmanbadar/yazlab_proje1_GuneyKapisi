using GuneyKapisi.Core;
using GuneyKapisi.NPC;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace GuneyKapisi.UI
{
    // DİKKAT: Bu script panelin kendisine değil, her zaman aktif olan Canvas'a eklenir.
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GuardNPC guard;
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text historyText;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private TMP_InputField inputField; 
        [SerializeField] private Button sendButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text thinkingText;
        private Coroutine _thinkingRoutine;
        public bool IsOpen => panel.activeSelf;
        private void Awake()
        {
            panel.SetActive(false);
            thinkingText.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(false);
            historyText.text = "";
            sendButton.onClick.AddListener(Submit);
            inputField.onSubmit.AddListener(_ => Submit()); // Enter ile gönder
            retryButton.onClick.AddListener(OnRetryClicked);
            closeButton.onClick.AddListener(Close);
        }
        private void OnEnable()
        {
            guard.NpcReplied += ShowNpcReply;
            guard.SystemMessage += ShowSystemMessage;
            guard.ThinkingChanged += ShowThinking;
            guard.ConversationEnded += Close;
            GameEvents.OnDoorOpened += Close;
            GameEvents.OnPlayerDefeated += Close;
        }
        private void OnDisable()
        {
            guard.NpcReplied -= ShowNpcReply;
            guard.SystemMessage -= ShowSystemMessage;
            guard.ThinkingChanged -= ShowThinking;
            guard.ConversationEnded -= Close;
            GameEvents.OnDoorOpened -= Close;
            GameEvents.OnPlayerDefeated -= Close;
        }
        public void Open()
        {
            if (IsOpen) return;
            panel.SetActive(true);
            inputField.ActivateInputField();
            GameEvents.RaiseDialogueToggled(true); // Kişi 1: oyuncu hareketini durdurur
        }
        public void Close()
        {
            if (!IsOpen) return;
            panel.SetActive(false);
            GameEvents.RaiseDialogueToggled(false);
        }
        private void Submit()
        {
            string text = inputField.text.Trim();
            if (text.Length == 0 || guard.IsBusy) return;
            AppendLine($"<b>Sen:</b> {text}");
            inputField.text = "";
            retryButton.gameObject.SetActive(false);
            guard.SendPlayerMessage(text);
            inputField.ActivateInputField();
        }
        private void OnRetryClicked()
        {
            retryButton.gameObject.SetActive(false);
            guard.Retry();
        }
        private void ShowNpcReply(string reply) =>
        AppendLine($"<b>{guard.NpcName}:</b> {reply}");
        private void ShowSystemMessage(string message, bool canRetry)
        {
            AppendLine($"<color=#E0A030><i>[Sistem] {message}</i></color>");
            retryButton.gameObject.SetActive(canRetry);
        }
        private void ShowThinking(bool thinking)
        {
            sendButton.interactable = !thinking;
            thinkingText.gameObject.SetActive(thinking);
            if (_thinkingRoutine != null) StopCoroutine(_thinkingRoutine);
            _thinkingRoutine = thinking ? StartCoroutine(AnimateThinking()) : null;
        }
        // "Gorvan düşünüyor", "Gorvan düşünüyor.", ".." , "..." döngüsü
        private IEnumerator AnimateThinking()
        {
            string baseText = $"{guard.NpcName} düşünüyor";
            int dots = 0;
            while (true)
            {
                thinkingText.text = baseText + new string('.', dots);
                dots = (dots + 1) % 4;
                yield return new WaitForSeconds(0.4f);
            }
        }
        private void AppendLine(string line)
        {
            historyText.text += line + "\n";
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f; // en alta kaydır
        }
    }
}
