using UnityEngine;
using Peco.Horror.GasStation;

namespace Peco.Horror.Gameplay
{
    /// <summary>
    /// Basic loop that simulates shifts throughout the night.
    /// It can be extended later with horror events or scripted encounters.
    /// </summary>
    public class GameLoopController : MonoBehaviour
    {
        [SerializeField] private PecoGasStationManager stationManager;
        [SerializeField, Tooltip("Intervalul în minute dintre verificările automate ale rafturilor.")]
        private float restockCheckInterval = 10f;

        private float timeSinceLastCheck;

        private void Update()
        {
            if (stationManager == null)
            {
                return;
            }

            timeSinceLastCheck += Time.deltaTime / 60f; // convert to in-game minutes
            if (timeSinceLastCheck >= restockCheckInterval)
            {
                stationManager.GenerateRestockPlan();
                timeSinceLastCheck = 0f;
            }
        }
    }
}
