using System;
using UnityEngine;
using GuneyKapisi.NPC;
using GuneyKapisi.UI;
namespace GuneyKapisi.Core
{
    // Oyunun akışını yöneten durum makinesi:
    // Bridge -> Camp -> Gate -> Won / Lost (Lost -> "Tekrar dene" -> Gate)
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EndScreenUI endScreen;
        [SerializeField] private GuardNPC guard;
        [Tooltip("Test_Gorvan sahnesinde Gate seç; Main sahnesinde Bridge")]
        [SerializeField] private GameState initialState = GameState.Bridge;
        public GameState State { get; private set; }
        public event Action<GameState> StateChanged;
        private void OnEnable()
        {
            GameEvents.OnObstacleCleared += HandleObstacleCleared;
            GameEvents.OnGateReached += HandleGateReached;
            GameEvents.OnDoorOpened += HandleDoorOpened;
            GameEvents.OnPlayerDefeated += HandlePlayerDefeated;
            endScreen.RetryClicked += HandleRetry;
        }
        private void OnDisable()
        {
            GameEvents.OnObstacleCleared -= HandleObstacleCleared;
            GameEvents.OnGateReached -= HandleGateReached;
            GameEvents.OnDoorOpened -= HandleDoorOpened;
            GameEvents.OnPlayerDefeated -= HandlePlayerDefeated;
            endScreen.RetryClicked -= HandleRetry;
        }
        private void Start() => SetState(initialState);
        private void HandleObstacleCleared()
        {
            if (State == GameState.Bridge) SetState(GameState.Camp);
        }
        private void HandleGateReached()
        {
            if (State == GameState.Camp) SetState(GameState.Gate);
        }
        private void HandleDoorOpened()
        {
            if (IsFinished) return;
            SetState(GameState.Won);
            endScreen.ShowWin();
        }
        private void HandlePlayerDefeated()
        {
            if (IsFinished) return;
            SetState(GameState.Lost);
            endScreen.ShowLose();
        }
        private void HandleRetry()
        {
            endScreen.HideAll();
            guard.ResetConversation();
            SetState(GameState.Gate);
        }
        private bool IsFinished => State == GameState.Won || State == GameState.Lost;
        private void SetState(GameState newState)
        {
            State = newState;
            Debug.Log($"[GameManager] Durum: {newState}");
            StateChanged?.Invoke(newState);
        }
    }
}
