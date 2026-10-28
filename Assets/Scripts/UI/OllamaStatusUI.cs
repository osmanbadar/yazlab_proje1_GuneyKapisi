using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GuneyKapisi.LLM;
namespace GuneyKapisi.UI
{
    // Ekranın köşesinde Ollama'nın durumunu gösterir: yeşil / sarı / kırmızı.
    public class OllamaStatusUI : MonoBehaviour
    {
        [SerializeField] private LLMSettings settings;
        [SerializeField] private Image statusDot;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private float checkIntervalSeconds = 10f;
        private ILLMClient _client;
        private CancellationTokenSource _cts;
        private void Awake() => _client = new OllamaClient(settings);
        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            _ = CheckLoopAsync(_cts.Token); // arka planda döngüyü başlat
        }
        private void OnDisable() => _cts.Cancel();
        private async Task CheckLoopAsync(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    LLMResult result = await _client.CheckHealthAsync(ct);
                    if (ct.IsCancellationRequested) return;
                    Show(result);
                    await Task.Delay(TimeSpan.FromSeconds(checkIntervalSeconds), ct);
                }
            }
            catch (OperationCanceledException) { } // obje kapandı, normal
            catch (Exception e) { Debug.LogException(e); }
        }
        private void Show(LLMResult result)
        {
            if (result.Success)
            {
                statusDot.color = Color.green; 
                statusText.text = $"LLM hazır ({settings.model})";
            }
            else if (result.ErrorType == LLMErrorType.ModelNotFound)
            {
                statusDot.color = Color.yellow;
                statusText.text = $"Model yüklü değil: {settings.model}";
            }
            else
            {
                statusDot.color = Color.red;
                statusText.text = "Ollama'ya ulaşılamıyor";
            }
        }
    }
}
