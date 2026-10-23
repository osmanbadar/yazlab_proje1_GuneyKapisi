using System.Collections.Generic;
using UnityEngine;
using GuneyKapisi.LLM;
namespace GuneyKapisi.Testing
{
   
    public class OllamaSmokeTest : MonoBehaviour
    {
        [SerializeField] private LLMSettings settings;
        private async void Start()
        {
            ILLMClient client = new OllamaClient(settings);
            Debug.Log("[SmokeTest] Sağlık kontrolü: " +
            (await client.CheckHealthAsync()).ErrorType);
            var messages = new List<ChatMessage>
 {
 new ChatMessage("user", "Bana Türkçe tek cümlelik bir selam ver.")
 };
            LLMResult result = await client.SendAsync(messages);
            Debug.Log(result.Success
            ? "[SmokeTest] Cevap: " + result.Content
            : $"[SmokeTest] Hata: {result.ErrorType} - {result.ErrorMessage}");
        }
    }
}