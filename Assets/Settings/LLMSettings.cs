using UnityEngine;
namespace GuneyKapisi.LLM
{
    [CreateAssetMenu(fileName = "LLMSettings", menuName = "GuneyKapisi/LLM Settings")]
    public class LLMSettings : ScriptableObject
    {
        [Tooltip("Ollama adresi. Başka bilgisayardaysa: http://IP-ADRESI:11434")]
        public string baseUrl = "http://127.0.0.1:11434";
        [Tooltip("ollama list komutunda görünen ad")]
        public string model = "qwen2.5:7b";
        [Range(0f, 1.5f)] public float temperature = 0.4f; 
        public int seed = 42; 
        [Tooltip("Saniye. İlk istekte model belleğe yüklenir, yavaş olabilir.")]
        public int timeoutSeconds = 60;
        [Tooltip("Modele gönderilecek en fazla geçmiş mesaj sayısı")]
        public int maxHistoryMessages = 10;
    }
}
