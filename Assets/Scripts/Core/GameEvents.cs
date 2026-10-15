using System;
namespace GuneyKapisi.Core
{
    public static class GameEvents
    {
        public static event Action OnObstacleCleared;
        public static event Action<string> OnItemCollected;
        public static event Action OnGateReached;
        public static event Action<bool> OnDialogueToggled;
        public static event Action OnDoorOpened;
        public static event Action OnPlayerDefeated;
        public static void RaiseObstacleCleared() => OnObstacleCleared?.Invoke(); 
        public static void RaiseItemCollected(string id) => OnItemCollected?.Invoke(id); 
        public static void RaiseGateReached() => OnGateReached?.Invoke(); 
        public static void RaiseDialogueToggled(bool open) => OnDialogueToggled?.Invoke(open); 
        public static void RaiseDoorOpened() => OnDoorOpened?.Invoke();
        public static void RaisePlayerDefeated() => OnPlayerDefeated?.Invoke();
    }
}