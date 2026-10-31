using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using GuneyKapisi.Companion;
using GuneyKapisi.Core;
namespace GuneyKapisi.Testing
{
    // Pip'i farklı başlangıç noktaları ve rastgele engel ayarlarıyla defalarca
    // çalıştırır; sonuçları CSV dosyasına yazar.
    public class CompanionTestRunner : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour companionSource; // Pip (ICommandable)
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private MonoBehaviour[] resettables; // IResettable
        [SerializeField] private MonoBehaviour[] randomizables; // IRandomizable
        [SerializeField] private int trialsPerSpawn = 10; 
        [SerializeField] private float trialTimeoutSeconds = 30f;
        [SerializeField] private int randomSeed = 12345;
        [SerializeField] private float timeScale = 2f; // testi hızlandırır
      private struct TrialResult
 {
 public int Spawn, Trial;
        public bool Success, TimedOut;
        public float Duration;
 }
 private IEnumerator Start()
        {
            var companion = companionSource as ICommandable;
            NavMeshAgent agent = companionSource != null
            ? companionSource.GetComponent<NavMeshAgent>()
            : null;
            if (companion == null || agent == null)
            {
                Debug.LogError("[TestRunner] Pip'te ICommandable ve NavMeshAgent olmalı.");
                yield break;
            }
            var rng = new System.Random(randomSeed); // aynı seed = aynı test dizisi
            var results = new List<TrialResult>();
            Time.timeScale = timeScale;
            yield return null; // diğer objelerin Start'ı bitsin
            for (int s = 0; s < spawnPoints.Length; s++)
            {
                for (int t = 0; t < trialsPerSpawn; t++)
                {
                    PrepareTrial(rng);
                    agent.Warp(spawnPoints[s].position); // Pip'i noktaya ışınla
                    yield return null;
                    float start = Time.time;
                    companion.ReceiveCommand(CompanionCommand.ActivateTarget);
                    yield return new WaitUntil(() =>
                    companion.Status == CompanionStatus.Success ||
                    companion.Status == CompanionStatus.Failed ||
                    Time.time - start > trialTimeoutSeconds);
                    float duration = Time.time - start;
                    results.Add(new TrialResult
                    {
                        Spawn = s,
                        Trial = t,
                        Success = companion.Status == CompanionStatus.Success,
                        TimedOut = duration > trialTimeoutSeconds,
                        Duration = duration
                    });
                    Debug.Log($"[TestRunner] Nokta {s + 1}, deneme {t + 1}: " +
                    $"{companion.Status} ({duration:F1} sn)");
                }
            }
            Time.timeScale = 1f;
            WriteCsv(results);
        }
        private void PrepareTrial(System.Random rng)
        {
            foreach (MonoBehaviour r in resettables)
                (r as IResettable)?.ResetState();
            foreach (MonoBehaviour r in randomizables)
                (r as IRandomizable)?.Randomize(rng);
        }
        private static void WriteCsv(List<TrialResult> results)
        {
            var sb = new StringBuilder("spawn;trial;success;timed_out;duration_s\n");
            int successCount = 0;
            float totalTime = 0f;
            foreach (TrialResult r in results)
            {
                string duration = r.Duration.ToString("F2", CultureInfo.InvariantCulture);
                sb.AppendLine(string.Join(";", r.Spawn + 1, r.Trial + 1, r.Success ? 1 : 0,
                r.TimedOut ? 1 : 0, duration));
                if (r.Success) { successCount++; totalTime += r.Duration; }
            }
            string path = Path.Combine(Application.persistentDataPath,
            $"companion_results_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            File.WriteAllText(path, sb.ToString());
            float rate = results.Count == 0 ? 0f : 100f * successCount / results.Count;
            float avg = successCount == 0 ? 0f : totalTime / successCount;
            Debug.Log($"[TestRunner] {results.Count} deneme | başarı %{rate:F1} | " +
            $"ortalama süre {avg:F2} sn (oyun zamanı) | dosya: {path}");
        }
    }
}
