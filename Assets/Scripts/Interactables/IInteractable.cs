using UnityEngine;

namespace GuneyKapisi.Interactables
{
    // Pip'in gidip etkileşime girebileceği objeler (şalter vb.).
    public interface IInteractable
    {
        Transform InteractionPoint { get; }   // Pip'in duracağı nokta
        bool IsActivated { get; }
        void Interact();
    }
}
