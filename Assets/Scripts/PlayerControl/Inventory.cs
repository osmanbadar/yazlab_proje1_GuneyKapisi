using System.Collections.Generic;
using UnityEngine;
using GuneyKapisi.Core;

namespace GuneyKapisi.PlayerControl
{
    public class Inventory : MonoBehaviour, IPlayerContextProvider
    {
        private readonly Dictionary<string, string> _items = new Dictionary<string, string>();

        public void Add(string id, string description)
        {
            if (_items.ContainsKey(id)) return;
            _items[id] = description;
            Debug.Log($"[Envanter] Eklendi: {description}");
            GameEvents.RaiseItemCollected(id);
        }

        public bool Has(string id) => _items.ContainsKey(id);

        public string GetContextForNpc() =>
            _items.Count == 0
                ? "Oyuncunun envanteri boş."
                : "Oyuncunun envanterinde: " + string.Join(", ", _items.Values);
    }
}