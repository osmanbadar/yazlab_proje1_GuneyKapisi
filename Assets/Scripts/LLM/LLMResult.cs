namespace GuneyKapisi.LLM
{
    public enum LLMErrorType
    {
        None, // hata yok
        ServerUnreachable, // Ollama kapalı / adres yanlış
        ModelNotFound, // model indirilmemiş
        Timeout, // cevap zamanında gelmedi
        BadResponse, // sunucudan anlamsız cevap
        Cancelled, // istek iptal edildi (obje yok edildi vb.)
        Unknown
    }
    // Bir LLM isteğinin sonucu. Hata durumunda exception fırlatmak yerine
    // bu nesne döner; böylece oyun hiçbir zaman çökmez.
    public class LLMResult
    {
        public bool Success { get; }
        public string Content { get; }
        public LLMErrorType ErrorType { get; }
        public string ErrorMessage { get; }
        private LLMResult(bool success, string content, LLMErrorType type, string message)
        {
            Success = success;
            Content = content;
            ErrorType = type;
            ErrorMessage = message;
        }
        public static LLMResult Ok(string content) =>
        new LLMResult(true, content, LLMErrorType.None, null);
        public static LLMResult Fail(LLMErrorType type, string message) =>
        new LLMResult(false, null, type, message);
    }
}
