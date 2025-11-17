using TMPro;
using UnityEngine;
using Peco.Horror.GasStation;
using Peco.Horror.World;

namespace Peco.Horror.UI
{
    /// <summary>
    /// Minimal HUD controller that binds world description and restock reports to UI labels.
    /// Requires a Canvas with TMP_Text components assigned.
    /// </summary>
    public class GasStationHud : MonoBehaviour
    {
        [SerializeField] private PecoGasStationManager stationManager;
        [SerializeField] private TMP_Text locationLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private TMP_Text ambientLabel;
        [SerializeField] private TMP_Text restockLabel;

        private void OnEnable()
        {
            if (stationManager != null)
            {
                stationManager.RestockReportChanged += HandleRestockReport;
                HandleRestockReport(stationManager.LastRestockReport);
            }
        }

        private void OnDisable()
        {
            if (stationManager != null)
            {
                stationManager.RestockReportChanged -= HandleRestockReport;
            }
        }

        private void Start()
        {
            if (stationManager == null)
            {
                Debug.LogWarning("GasStationHud nu are configurat PecoGasStationManager.");
                return;
            }

            ApplyWorldDescriptor(stationManager.WorldDescriptor);
            stationManager.GenerateRestockPlan();
            RefreshRestockReport();
        }

        public void ApplyWorldDescriptor(WorldDescriptor descriptor)
        {
            if (descriptor == null)
            {
                return;
            }

            if (locationLabel != null)
            {
                locationLabel.text = descriptor.LocationName;
            }

            if (descriptionLabel != null)
            {
                descriptionLabel.text = descriptor.Description;
            }

            if (ambientLabel != null)
            {
                ambientLabel.text = descriptor.AmbientNotes;
            }
        }

        public void RefreshRestockReport()
        {
            if (restockLabel == null || stationManager == null)
            {
                return;
            }

            restockLabel.text = stationManager.LastRestockReport;
        }

        private void HandleRestockReport(string message)
        {
            if (restockLabel != null)
            {
                restockLabel.text = message;
            }
        }
    }
}
