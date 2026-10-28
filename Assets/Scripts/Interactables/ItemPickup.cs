using UnityEngine;
using GuneyKapisi.PlayerControl;

namespace GuneyKapisi.Interactables
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private string itemId = "letter";
        [SerializeField] private string description = "muhtarın mühürlü mektubu";

        private void Reset() => GetComponent<Collider>().isTrigger = true;

        private void Update() => transform.Rotate(0f, 60f * Time.deltaTime, 0f);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!other.TryGetComponent(out Inventory inventory)) return;
            inventory.Add(itemId, description);
            gameObject.SetActive(false);
        }
    }
}