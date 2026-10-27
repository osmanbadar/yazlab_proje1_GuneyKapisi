using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using GuneyKapisi.Core;
using GuneyKapisi.LLM;
namespace GuneyKapisi.NPC
{
    public class GuardNPC : MonoBehaviour
    {
        [Header("Ayarlar")]
        [SerializeField] private LLMSettings settings;
        [SerializeField] private NpcPersona persona;
        [SerializeField] private int maxPlayerMessages = 15;
        [Header("Bağlantılar (arayüzü uygulayan bileşeni sürükle)")]
        [SerializeField] private MonoBehaviour actionHandlerSource; // INpcActionHandler
        [SerializeField] private MonoBehaviour contextProviderSource; // IPlayerContextProvider
        public event Action<string> NpcReplied; // Gorvan'ın sözü
        public event Action<string, bool> SystemMessage; // mesaj, tekrar denenebilir mi
        public event Action<bool> ThinkingChanged; // düşünüyor mu
        public event Action ConversationEnded;
        public bool IsBusy { get; private set; }
        public string NpcName => persona.npcName; 
        private ILLMClient _client;
        private IResponseParser<NpcResponse> _parser;
        private INpcActionHandler _actionHandler;
        private IPlayerContextProvider _contextProvider;
        private readonly List<ChatMessage> _history = new List<ChatMessage>();
        private CancellationTokenSource _cts;
        private int _playerMessageCount;
        private bool _canRetry;
        private void Awake()
        {
            // Bağımlılıklar tek yerde kuruluyor; sınıfın geri kalanı yalnızca
            // arayüzleri kullanıyor (SOLID - Dependency Inversion).
            _client = new OllamaClient(settings, NpcResponseSchema.Create());
            _parser = new NpcResponseParser();
            _actionHandler = actionHandlerSource as INpcActionHandler;
            _contextProvider = contextProviderSource as IPlayerContextProvider;
            _cts = new CancellationTokenSource();
            if (_actionHandler == null)
                Debug.LogError("[GuardNPC] Action Handler Source alanına " +
                "INpcActionHandler uygulayan bir bileşen sürükle.");
        }
        private void OnDestroy() => _cts.Cancel();
        // DialogueUI çağırır.
        public async void SendPlayerMessage(string text)
        {
            if (IsBusy || string.IsNullOrWhiteSpace(text)) return;
            if (_playerMessageCount >= maxPlayerMessages)
            {
                SystemMessage?.Invoke($"{NpcName} artık seninle konuşmak istemiyor.", false);
                ConversationEnded?.Invoke();
                return;
            }
            _playerMessageCount++;
            _history.Add(new ChatMessage("user", text.Trim()));
            await RequestReplyAsync();
        }
        // Hata sonrası "Tekrar Dene" düğmesi çağırır; aynı geçmişle yeniden sorar.
        public async void Retry()
        {
            if (IsBusy || !_canRetry) return;
            await RequestReplyAsync();
        }
        // Kaybettikten sonra "Tekrar dene" ile sohbet sıfırlanır.
        public void ResetConversation()
        {
            _history.Clear();
            _playerMessageCount = 0;
            _canRetry = false;
        }
        private async Task RequestReplyAsync()
        {
            SetBusy(true);
            _canRetry = false;
            try
            {
                LLMResult result = null;
                NpcResponse response = null;
                // Bozuk JSON gelirse bir kez daha sorar (toplam 2 deneme).
                for (int attempt = 1; attempt <= 2; attempt++)
                {
                    result = await _client.SendAsync(BuildMessages(), _cts.Token);
                    if (this == null || result.ErrorType == LLMErrorType.Cancelled) return;
                    if (!result.Success) break;
                    if (_parser.TryParse(result.Content, out response)) break;
                    Debug.LogWarning($"[GuardNPC] Geçersiz cevap ({attempt}): " +
                    result.Content);
                }
                if (!result.Success)
                {
                    Fail(ToUserMessage(result));
                    return;
                }
                if (response == null)
                {
                    Fail("Bekçinin cevabı anlaşılamadı. Tekrar dene.");
                    return;
                }
                Debug.Log($"[GuardNPC] Model çıktısı: {result.Content}");
                _history.Add(new ChatMessage("assistant", result.Content));
                NpcReplied?.Invoke(response.Reply);
                _actionHandler?.Handle(response.Action);
                if (response.Action == NpcAction.EndConversation)
                    ConversationEnded?.Invoke();
            }
            catch (Exception e) // beklenmeyen her şey: oyun yine de çökmez
            {
                Debug.LogException(e);
                Fail("Beklenmeyen bir hata oluştu. Tekrar dene.");
            }
            finally
            {
                if (this != null) SetBusy(false);
            }
        }
        // Modele gidecek mesajlar: system prompt + (varsa) oyun bilgisi + son geçmiş.
        private List<ChatMessage> BuildMessages()
        {
            var messages = new List<ChatMessage>
 {
 new ChatMessage("system", persona.systemPrompt)
 };
            string context = _contextProvider?.GetContextForNpc();
            if (!string.IsNullOrWhiteSpace(context))
                messages.Add(new ChatMessage("system", "Oyun bilgisi: " + context));
            int start = Mathf.Max(0, _history.Count - settings.maxHistoryMessages);
            for (int i = start; i < _history.Count; i++)
                messages.Add(_history[i]);
            return messages;
        }
        private string ToUserMessage(LLMResult r)
        {
            switch (r.ErrorType)
            {
                case LLMErrorType.ServerUnreachable:
                    return "Bekçiyle iletişim kurulamıyor: Ollama sunucusuna ulaşılamadı. " +
                    "Ollama'nın açık olduğunu kontrol edip tekrar dene.";
                case LLMErrorType.ModelNotFound:
                    return $"'{settings.model}' modeli yüklü değil. Terminalde " +
                    $"'ollama pull {settings.model}' komutunu çalıştır.";
                case LLMErrorType.Timeout:
                    return "Bekçi çok uzun süre düşündü (zaman aşımı). Tekrar dene.";
                case LLMErrorType.BadResponse:
                    return "Sunucudan beklenmeyen bir cevap geldi. Tekrar dene.";
                default:
                    Debug.LogWarning($"[GuardNPC] {r.ErrorType}: {r.ErrorMessage}");
                    return "Beklenmeyen bir hata oluştu. Tekrar dene.";
            }
        }
        private void Fail(string message)
        {
            _canRetry = true;
            SystemMessage?.Invoke(message, true);
        }
        private void SetBusy(bool busy)
        {
            IsBusy = busy;
            ThinkingChanged?.Invoke(busy);
        }
    }
}