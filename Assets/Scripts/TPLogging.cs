using Oculus.Interaction;
using Oculus.Interaction.PoseDetection.Debug;
using UnityEngine;

public class TPLogging : MonoBehaviour
{

    // For example timer
    private float elapsedTime = 0.0f;
    private float elapsedTime_mins = 0.0f;
    private float elapsedTime_secs = 0.0f;
    [SerializeField] private Axis2DActiveState activeState;
    [SerializeField] private FlagManager flagManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool startedTimer = false;      // This bool is the important one. Tells when the first TP is taken
    private bool hasLoggedTeleport = false; // Add this to prevent multiple logs
    void Start()
    {
        if (flagManager == null)
            flagManager = FindObjectOfType<FlagManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If a teleport is broadcasted, start a timer
        if (activeState.Active && !startedTimer)
        {
            startedTimer = true;

            // Log the first teleport for this trial
            if (!hasLoggedTeleport && flagManager != null)
            {
                int currentTrial = flagManager.CurrentTrialNumber;

                // Use DataLogger to start timer and log the event
                DataLogger.StartTimer(currentTrial, "TeleportStart");

                // Optionally log it as teleport specifically if needed
                // DataLogger.LogTeleportStart(currentTrial);

                hasLoggedTeleport = true;
                Debug.Log($"First teleport detected in trial {currentTrial}");
            }
        }
    }

    // Restart timer & variable that keeps track of timer when called (used in EditorController)
    public void PrepareNextTrial() {
        // Call any needed methods to prepare next trial (such as restarting time)
        startedTimer = false;
        hasLoggedTeleport = false; // Reset the logging flag
        elapsedTime = 0.0f;
    }


    // Was used for debugging
    public string getTimerText() {
        return string.Format("{0:00}:{1:00}", elapsedTime_mins, elapsedTime_secs);
    }
}
