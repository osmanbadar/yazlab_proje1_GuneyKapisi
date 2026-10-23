using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
namespace GuneyKapisi.LLM
{
    
    public class OllamaClient : ILLMClient
    {
        private readonly LLMSettings _settings;
        private readonly JObject _responseSchema; 
        public OllamaClient(LLMSettings settings, JObject responseSchema = null)
        {
            _settings = settings;
            _responseSchema = responseSchema;
        }
        public async Task<LLMResult> SendAsync(IReadOnlyList<ChatMessage> messages,
        CancellationToken ct = default)
        {
            var body = new JObject
            {
                ["model"] = _settings.model,
                ["stream"] = false, 
                ["keep_alive"] = "30m", 
                ["messages"] = JArray.FromObject(messages),
                ["options"] = new JObject
                {
                    ["temperature"] = _settings.temperature,
                    ["seed"] = _settings.seed
                }
            };
            if (_responseSchema != null)
                body["format"] = _responseSchema; 
            using var request = new UnityWebRequest(Url("/api/chat"), "POST");
            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString(Formatting.None));
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = _settings.timeoutSeconds;
            LLMResult sendError = await SendSafelyAsync(request, ct);
            if (sendError != null) return sendError;
            if (request.result != UnityWebRequest.Result.Success)
                return ToError(request);
            return ExtractContent(request.downloadHandler.text);
        }
        public async Task<LLMResult> CheckHealthAsync(CancellationToken ct = default)
        {
            using var request = UnityWebRequest.Get(Url("/api/tags"));
            request.timeout = 5;
            LLMResult sendError = await SendSafelyAsync(request, ct);
            if (sendError != null) return sendError;
            if (request.result != UnityWebRequest.Result.Success)
                return LLMResult.Fail(LLMErrorType.ServerUnreachable, request.error);
            try
            {
                
                var models = JObject.Parse(request.downloadHandler.text)["models"] as JArray;
                if (models != null)
                    foreach (JToken m in models)
                        if (IsSameModel((string)m["name"]))
                            return LLMResult.Ok("ok");
                return LLMResult.Fail(LLMErrorType.ModelNotFound, _settings.model);
            }
            catch (Exception e)
            {
                return LLMResult.Fail(LLMErrorType.BadResponse, e.Message);
            }
        }
        
        private static async Task<LLMResult> SendSafelyAsync(UnityWebRequest request,
        CancellationToken ct)
        {
            try
            {
                using (ct.Register(request.Abort))
                    await request.SendAndWaitAsync();
                return ct.IsCancellationRequested
                ? LLMResult.Fail(LLMErrorType.Cancelled, "İstek iptal edildi.")
                : null;
            }
            catch (Exception e)
            {
                return LLMResult.Fail(LLMErrorType.Unknown, e.Message);
            }
        }
        private static LLMResult ToError(UnityWebRequest request)
        {
            string error = request.error ?? "";
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                bool timeout =
                error.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                error.IndexOf("timed out", StringComparison.OrdinalIgnoreCase) >= 0;
                return LLMResult.Fail(
                timeout ? LLMErrorType.Timeout : LLMErrorType.ServerUnreachable, error);
            }
            if (request.responseCode == 404)
                return LLMResult.Fail(LLMErrorType.ModelNotFound, error);
            return LLMResult.Fail(LLMErrorType.BadResponse,
            $"HTTP {request.responseCode}: {error}");
        }
        
        private static LLMResult ExtractContent(string json)
        {
            try
            {
                string content = (string)JObject.Parse(json)["message"]?["content"];
                return string.IsNullOrWhiteSpace(content)
                ? LLMResult.Fail(LLMErrorType.BadResponse, "Model boş cevap döndürdü.")
                : LLMResult.Ok(content);
            }
            catch (Exception e)
            {
                return LLMResult.Fail(LLMErrorType.BadResponse, e.Message);
            }
        }
        private bool IsSameModel(string name) =>
        name == _settings.model || name == _settings.model + ":latest";
        private string Url(string path) => _settings.baseUrl.TrimEnd('/') + path;
    }
}
