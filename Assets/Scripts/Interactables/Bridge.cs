using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using GuneyKapisi.Core;

namespace GuneyKapisi.Interactables
{
    // Kalkık köprü. Lower() ile iner; inince NavMesh yenilenir ve
    // GameEvents.OnObstacleCleared tetiklenir (oyun Kamp bölümüne geçer).
    public class Bridge : MonoBehaviour, IResettable
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private Vector3 raisedEuler = new Vector3(-80f, 0f, 0f);
        [SerializeField] private float lowerDuration = 2f;
        [SerializeField] private NavMeshSurface[] surfacesToRebuild;
        [SerializeField] private GameObject edgeBlocker;   // köprü önündeki görünmez duvar
        [SerializeField] private bool raiseGameEvent = true;

        private Coroutine _routine;

        public bool IsLowered { get; private set; }

        private void Awake() => SetRaised();

        public void Lower()
        {
            if (IsLowered) return;
            IsLowered = true;
            _routine = StartCoroutine(LowerRoutine());
        }

        private IEnumerator LowerRoutine()
        {
            Quaternion from = Quaternion.Euler(raisedEuler);
            for (float t = 0f; t < 1f; t += Time.deltaTime / lowerDuration)
            {
                pivot.localRotation = Quaternion.Slerp(from, Quaternion.identity, t);
                yield return null;
            }
            pivot.localRotation = Quaternion.identity;
            if (edgeBlocker != null) edgeBlocker.SetActive(false);   // oyuncu geçebilsin
            RebuildNavMesh();
            if (raiseGameEvent) GameEvents.RaiseObstacleCleared();
        }

        public void ResetState()
        {
            if (_routine != null) StopCoroutine(_routine);
            SetRaised();
            RebuildNavMesh();
        }

        private void SetRaised()
        {
            IsLowered = false;
            pivot.localRotation = Quaternion.Euler(raisedEuler);
            if (edgeBlocker != null) edgeBlocker.SetActive(true);
        }

        private void RebuildNavMesh()
        {
            foreach (NavMeshSurface surface in surfacesToRebuild)
                surface.BuildNavMesh();
        }
    }
}