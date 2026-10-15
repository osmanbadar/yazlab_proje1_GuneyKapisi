using UnityEngine;
using GuneyKapisi.Core;

namespace GuneyKapisi.Interactables
{
    public class Lever : MonoBehaviour, IInteractable, IResettable
    {
        [SerializeField] private Transform handle;
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private Bridge bridge;
        [SerializeField] private Vector3 pulledEuler = new Vector3(60f, 0f, 0f);

        private Quaternion _startRotation;

        public Transform InteractionPoint => interactionPoint;
        public bool IsActivated { get; private set; }

        private void Awake() => _startRotation = handle.localRotation;

        public void Interact()
        {
            if (IsActivated) return;
            IsActivated = true;
            handle.localRotation = _startRotation * Quaternion.Euler(pulledEuler);
            bridge.Lower();
        }

        public void ResetState()
        {
            IsActivated = false;
            handle.localRotation = _startRotation;
        }
    }
}