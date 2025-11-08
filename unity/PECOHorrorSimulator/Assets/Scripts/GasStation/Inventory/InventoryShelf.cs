using System.Collections.Generic;
using UnityEngine;

namespace Peco.Horror.Inventory
{
    /// <summary>
    /// Represents a physical shelf inside the minimarket.
    /// Slots can be configured in the Inspector with ScriptableObjects created from InventoryItemDefinition.
    /// </summary>
    public class InventoryShelf : MonoBehaviour
    {
        [SerializeField] private string shelfName = "Raft Produse Locale";
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public string ShelfName => shelfName;
        public IReadOnlyList<InventorySlot> Slots => slots;

        public InventorySlot FindSlot(InventoryItemDefinition item)
        {
            return slots.Find(slot => slot.Item == item);
        }

        public void RestockSlot(InventoryItemDefinition item, int amount)
        {
            var slot = FindSlot(item);
            if (slot == null)
            {
                return;
            }

            slot.Add(amount);
        }

        public int Withdraw(InventoryItemDefinition item, int amount)
        {
            var slot = FindSlot(item);
            return slot == null ? 0 : slot.Remove(amount);
        }
    }
}
