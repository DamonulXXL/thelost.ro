using UnityEngine;

namespace Peco.Horror.Inventory
{
    /// <summary>
    /// Scriptable representation for a minimarket product.
    /// Designers can create multiple assets for food, drinks or fuel accessories
    /// and tweak prices or restock defaults without touching code.
    /// </summary>
    [CreateAssetMenu(menuName = "Peco/Inventory Item", fileName = "InventoryItem")]
    public class InventoryItemDefinition : ScriptableObject
    {
        [SerializeField] private string displayName = "Cafea la filtru";
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0f)] private float unitPrice = 12.5f;
        [SerializeField, Range(1, 200)] private int shelfCapacity = 40;
        [SerializeField, Range(1, 200)] private int defaultRestockAmount = 20;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public float UnitPrice => unitPrice;
        public int ShelfCapacity => shelfCapacity;
        public int DefaultRestockAmount => defaultRestockAmount;
    }
}
