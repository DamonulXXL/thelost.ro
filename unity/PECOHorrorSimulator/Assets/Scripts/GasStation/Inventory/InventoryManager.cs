using System;
using System.Collections.Generic;
using UnityEngine;

namespace Peco.Horror.Inventory
{
    /// <summary>
    /// Central orchestrator for shelf management and depot integration.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<InventoryShelf> shelves = new List<InventoryShelf>();
        [SerializeField, Tooltip("Capacitatea depozitului din spate pentru cutii sigilate.")]
        private int depotCapacity = 500;
        [SerializeField, Tooltip("Cantitatea curentă disponibilă în depozit pentru refacerea rafturilor.")]
        private int depotStock = 200;

        public event Action<InventoryShelf> OnShelfRestocked;

        public IReadOnlyList<InventoryShelf> Shelves => shelves;
        public int DepotCapacity => depotCapacity;
        public int DepotStock => depotStock;

        public void AddDepotStock(int amount)
        {
            depotStock = Mathf.Clamp(depotStock + Mathf.Max(0, amount), 0, depotCapacity);
        }

        public void ConsumeDepotStock(int amount)
        {
            depotStock = Mathf.Clamp(depotStock - Mathf.Max(0, amount), 0, depotCapacity);
        }

        public bool TryRestockShelf(InventoryShelf shelf, InventoryItemDefinition item, int requestedAmount, out int appliedAmount)
        {
            appliedAmount = 0;
            if (shelf == null || item == null || requestedAmount <= 0)
            {
                return false;
            }

            var slot = shelf.FindSlot(item);
            if (slot == null)
            {
                return false;
            }

            var amount = Mathf.Min(requestedAmount, slot.RemainingCapacity, depotStock);
            if (amount <= 0)
            {
                return false;
            }

            slot.Add(amount);
            ConsumeDepotStock(amount);
            appliedAmount = amount;
            OnShelfRestocked?.Invoke(shelf);
            return true;
        }
    }
}
