using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GuneyKapisi.LLM
{
    // Herhangi bir dil modeli istemcisinin sözleşmesi.
    public interface ILLMClient
    {
        // Mesajları gönderir, modelin cevabını (ham metin) döndürür.
        Task<LLMResult> SendAsync(IReadOnlyList<ChatMessage> messages,
        CancellationToken ct = default);
        // Sunucu açık mı ve model yüklü mü?
        Task<LLMResult> CheckHealthAsync(CancellationToken ct = default);
    }
}
