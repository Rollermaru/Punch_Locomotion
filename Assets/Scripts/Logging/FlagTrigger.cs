using Unity.Logging;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagTrigger : MonoBehaviour
{
    public FlagManager flagManager;
    private bool hasTriggered = false;

    [Header("Flag Proximity")]
    [SerializeField] private Transform[] flags;
    private bool[] hasLoggedFlag;
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        // Find the flag manager in the scene
        // flagManager = FindObjectOfType<FlagManager>();
        // if (flagManager == null)
        // {
        //     Debug.LogError("FlagManager not found in scene!");
        // }
    }
    private void OnEnable()
    {
        // Reset trigger state when flag is activated
        hasTriggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Make sure we're only registering the hit once
        if (!hasTriggered)
        {
            hasTriggered = true;

            // Find the FlagManager and notify it
            // FlagManager flagManager = FindObjectOfType<FlagManager>();
            if (flagManager != null)
            {
                // call FlagReached with this GameObject
                DataLogging();
            }
            else
            {
                Debug.LogError("FlagManager not found!");
            }
        }
    }

    private void DataLogging() {
        flagManager.FlagReached(this.gameObject);
        Debug.Log($"Flag '{name}' triggered FlagManager.FlagReached");

        // If dashing is happening, Dash.cs will call DataLogger.LogTeleportStart()
        // So here, we only handle "flag hit" detection logic
        for (int i = 0; i < flags.Length; i++)
        {
            // Skip if already logged or if the flag is inactive
            if (hasLoggedFlag[i] || !flags[i].gameObject.activeInHierarchy) continue;

            float d = Vector3.Distance(transform.position, playerTransform.position);

            if (d <= 5f)  // Within a 5-meter radius
            {
                hasLoggedFlag[i] = true;

                // Get the current trial number from FlagManager
                int currentTrial = 0;
                if (flagManager != null)
                {
                    currentTrial = flagManager.CurrentTrialNumber;
                }

                // Log the event with the trial number
                Debug.Log("LOG THE FLAG HIT");
                DataLogger.LogFlagHit(currentTrial, flags[i].name, flags[i].position, transform.position);

                Debug.Log($"[Distance] Flag '{flags[i].name}' hit at t={Time.time:F2}, d={d:F2}, Trial={currentTrial}");

                if (flagManager != null)
                    flagManager.FlagReached(flags[i].gameObject);
            }
        }
    }

}
