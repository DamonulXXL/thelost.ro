using System;
using System.Collections.Generic;
using UnityEngine;
using Peco.Horror.Inventory;
using Peco.Horror.Restock;
using Peco.Horror.World;

namespace Peco.Horror.GasStation
{
    /// <summary>
    /// High level orchestrator that wires pumps, inventory and world data together.
    /// Attach this to an empty GameObject called "P.E.C.O. Station" inside the entry scene.
    /// </summary>
    public class PecoGasStationManager : MonoBehaviour
    {
        [Header("Configurație")] 
        [SerializeField] private WorldDescriptor worldDescriptor;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private RestockCalculator restockCalculator;
        [SerializeField] private List<FuelPump> fuelPumps = new List<FuelPump>();

        [Header("Monitorizare")] 
        [SerializeField, TextArea] private string lastRestockReport;

        public event Action<string> RestockReportChanged;

        public WorldDescriptor WorldDescriptor => worldDescriptor;
        public IReadOnlyList<FuelPump> FuelPumps => fuelPumps;
        public InventoryManager InventoryManager => inventoryManager;
        public string LastRestockReport => lastRestockReport;

        private void Awake()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnShelfRestocked += HandleShelfRestocked;
            }
        }

        private void OnDestroy()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnShelfRestocked -= HandleShelfRestocked;
            }
        }

        public float DispenseFuel(string pumpId, float liters)
        {
            var pump = fuelPumps.Find(p => p.PumpId == pumpId);
            return pump == null ? 0f : pump.Dispense(liters);
        }

        public void GenerateRestockPlan()
        {
            if (inventoryManager == null || restockCalculator == null)
            {
                SetRestockReport("Calculatorul de restock nu este configurat.");
                return;
            }

            var allSlots = new List<InventorySlot>();
            foreach (var shelf in inventoryManager.Shelves)
            {
                allSlots.AddRange(shelf.Slots);
            }

            var plan = restockCalculator.BuildPlan(allSlots);
            if (plan.Count == 0)
            {
                SetRestockReport("Rafturile sunt pregătite pentru seara aglomerată.");
                return;
            }

            var report = "Plan restock P.E.C.O.:\n";
            foreach (var entry in plan)
            {
                var slot = entry.Key;
                report += $"- {slot.Item.DisplayName}: +{entry.Value} bucăți (în prezent {slot.Quantity}).\n";
            }

            SetRestockReport(report);
        }

        private void HandleShelfRestocked(InventoryShelf shelf)
        {
            SetRestockReport($"{shelf.ShelfName} a fost realimentat. Depozit rămas: {inventoryManager.DepotStock}.");
        }

        private void SetRestockReport(string message)
        {
            lastRestockReport = message;
            RestockReportChanged?.Invoke(message);
        }
    }
}
