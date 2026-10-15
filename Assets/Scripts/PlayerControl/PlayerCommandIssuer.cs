using UnityEngine;
using UnityEngine.InputSystem;
using GuneyKapisi.Companion;

namespace GuneyKapisi.PlayerControl
{
    // Uçurumun önündeki görünmez alan. Oyuncu içindeyken E → Pip'e emir.
    [RequireComponent(typeof(Collider))]
    public class PlayerCommandIssuer : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour companionSource;   // ICommandable (Pip)
        [SerializeField] private GameObject hint;                 // "Pip'e emir vermek için E"

        private ICommandable _companion;
        private bool _playerInside;

        private void Reset() => GetComponent<Collider>().isTrigger = true;

        private void Awake()
        {
            _companion = companionSource as ICommandable;
            hint.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInside = true;
            hint.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInside = false;
            hint.SetActive(false);
        }

        private void Update()
        {
            if (!_playerInside || Keyboard.current == null) return;
            if (Keyboard.current.eKey.wasPressedThisFrame)
                _companion.ReceiveCommand(CompanionCommand.ActivateTarget);
        }
    }
}