using UnityEngine;

public class PunchTrialManager : MonoBehaviour
{
    [SerializeField] private ActivateTP activateTP;
    [SerializeField] private FlagManager flagManager;

    private float punchStartTime;
    private bool timerStarted = false;
    private bool hasLoggedFirstPunch = false; // Add this new variable

    void Start()
    {
        activateTP.OnFirstPunch += HandleFirstPunch;
        // Subscribe to trial start events
        if (flagManager != null)
        {
            flagManager.OnTrialStarted += ResetTrial;
        }
    }

    void OnDestroy()
    {
        activateTP.OnFirstPunch -= HandleFirstPunch;
    }

    private void HandleFirstPunch()
    {
        // Only log the first punch of the trial
        if (!hasLoggedFirstPunch)
        {
            punchStartTime = Time.time;
            timerStarted = true;

            // Log the punch start time only once
            if (flagManager != null)
            {
                DataLogger.StartTimer(flagManager.CurrentTrialNumber);
            }

            hasLoggedFirstPunch = true; // Prevent further logging
            Debug.Log($"First punch logged at {punchStartTime}");
        }
    }

    // Call this at the start of every trial
    public void ResetTrial()
    {
        timerStarted = false;
        hasLoggedFirstPunch = false; // Reset this for the next trial
        activateTP.ResetPunchFlag();
    }

    // Get time since punch
    public float GetTimeSincePunch()
    {
        if (!timerStarted) return 0f;
        return Time.time - punchStartTime;
    }
}
