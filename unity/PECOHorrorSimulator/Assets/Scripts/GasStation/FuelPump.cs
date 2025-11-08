using UnityEngine;

namespace Peco.Horror.GasStation
{
    /// <summary>
    /// Represents a single fuel pump island and basic billing calculation.
    /// </summary>
    public class FuelPump : MonoBehaviour
    {
        [SerializeField] private string pumpId = "Pompă #1";
        [SerializeField] private string fuelType = "Benzină 95";
        [SerializeField, Min(0f)] private float pricePerLiter = 7.58f;
        [SerializeField, Min(0f)] private float vatPercent = 19f;
        [SerializeField, Min(0f)] private float capacity = 4000f;
        [SerializeField, Min(0f)] private float currentVolume = 3200f;

        public string PumpId => pumpId;
        public string FuelType => fuelType;
        public float PricePerLiter => pricePerLiter;
        public float VatPercent => vatPercent;
        public float Capacity => capacity;
        public float CurrentVolume => currentVolume;

        public bool CanDispense(float liters) => liters > 0 && currentVolume >= liters;

        public float Dispense(float liters)
        {
            if (!CanDispense(liters))
            {
                return 0f;
            }

            currentVolume = Mathf.Max(0f, currentVolume - liters);
            return CalculateTotal(liters);
        }

        public float CalculateTotal(float liters)
        {
            var subtotal = liters * pricePerLiter;
            var vat = subtotal * (vatPercent / 100f);
            return subtotal + vat;
        }

        public void Refill(float liters)
        {
            currentVolume = Mathf.Clamp(currentVolume + Mathf.Max(0f, liters), 0f, capacity);
        }
    }
}
