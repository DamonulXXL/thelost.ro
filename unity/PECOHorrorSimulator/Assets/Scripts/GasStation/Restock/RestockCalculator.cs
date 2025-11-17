using System.Collections.Generic;
using UnityEngine;
using Peco.Horror.Inventory;

namespace Peco.Horror.Restock
{
    /// <summary>
    /// Provides formulas for planning restock quantities based on shelf levels and upcoming events.
    /// </summary>
    [CreateAssetMenu(menuName = "Peco/Restock Calculator", fileName = "RestockCalculator")]
    public class RestockCalculator : ScriptableObject
    {
        [Tooltip("Factor de multiplicare aplicat nivelului standard de restock.")]
        [SerializeField, Range(0.5f, 5f)] private float demandMultiplier = 1.0f;
        [Tooltip("Rezervă de siguranță aplicată peste cantitatea calculată.")]
        [SerializeField, Range(0, 50)] private int safetyBuffer = 5;

        /// <summary>
        /// Returnează un plan de restock pentru fiecare slot.
        /// </summary>
        public Dictionary<InventorySlot, int> BuildPlan(IEnumerable<InventorySlot> slots)
        {
            var plan = new Dictionary<InventorySlot, int>();

            foreach (var slot in slots)
            {
                if (slot?.Item == null)
                {
                    continue;
                }

                var definition = slot.Item;
                var baseline = Mathf.CeilToInt(definition.DefaultRestockAmount * demandMultiplier);
                var suggested = Mathf.Clamp(baseline + safetyBuffer, 0, definition.ShelfCapacity);
                var remaining = Mathf.Max(0, suggested - slot.Quantity);

                if (remaining > 0)
                {
                    plan[slot] = remaining;
                }
            }

            return plan;
        }
    }
}
