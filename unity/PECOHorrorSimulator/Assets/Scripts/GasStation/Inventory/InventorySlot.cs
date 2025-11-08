using System;
using UnityEngine;

namespace Peco.Horror.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private InventoryItemDefinition item;
        [SerializeField, Min(0)] private int quantity;

        public InventoryItemDefinition Item => item;
        public int Quantity => quantity;

        public bool IsEmpty => item == null || quantity <= 0;

        public int RemainingCapacity => item == null ? 0 : Mathf.Max(0, item.ShelfCapacity - quantity);

        public void Configure(InventoryItemDefinition definition, int startingQuantity)
        {
            item = definition;
            quantity = Mathf.Clamp(startingQuantity, 0, definition != null ? definition.ShelfCapacity : 0);
        }

        public void Add(int amount)
        {
            if (item == null || amount <= 0)
            {
                return;
            }

            quantity = Mathf.Clamp(quantity + amount, 0, item.ShelfCapacity);
        }

        public int Remove(int amount)
        {
            if (IsEmpty || amount <= 0)
            {
                return 0;
            }

            var removed = Mathf.Min(quantity, amount);
            quantity -= removed;
            return removed;
        }
    }
}
