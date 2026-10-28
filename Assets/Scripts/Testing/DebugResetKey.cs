using UnityEngine;
using UnityEngine.InputSystem;
using GuneyKapisi.Core;

namespace GuneyKapisi.Testing
{
    public class DebugResetKey : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.rKey.wasPressedThisFrame) return;

            foreach (var mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (mb is IResettable r) r.ResetState();

            Debug.Log("[Test] Sahne sıfırlandı");
        }
    }
}