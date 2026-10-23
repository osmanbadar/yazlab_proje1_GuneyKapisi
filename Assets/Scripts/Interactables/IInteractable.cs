using UnityEngine;

namespace GuneyKapisi.Interactables
{
    public interface IInteractable
    {
        Transform InteractionPoint { get; }
        bool IsActivated { get; }
        void Interact();
    }
}
