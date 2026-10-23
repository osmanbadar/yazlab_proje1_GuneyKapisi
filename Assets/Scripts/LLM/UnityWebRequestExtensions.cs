using System.Threading.Tasks;
using UnityEngine.Networking;
namespace GuneyKapisi.LLM
{
    internal static class UnityWebRequestExtensions
    {
        
        public static Task SendAndWaitAsync(this UnityWebRequest request)
        {
            var tcs = new TaskCompletionSource<bool>();
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            operation.completed += _ => tcs.TrySetResult(true);
            return tcs.Task;
        }
    }
}
