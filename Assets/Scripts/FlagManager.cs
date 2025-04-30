using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [SerializeField] private GameObject[] flagObjects; // All flag objects
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool[] flagUsed;

    // Use HashSet for efficient lookup of used flags
    private HashSet<int> usedFlagIndices = new HashSet<int>();
    private int currentFlagIndex = -1;
    private int currentTrialNumber = 0;
    private bool trialInProgress = false;
    private bool experimentComplete = false;

    // Public accessor for current trial number
    public int CurrentTrialNumber { get { return currentTrialNumber; } }

    void Start()
    {
        // Initialize the flagUsed array
        flagUsed = new bool[flagObjects.Length];
        for (int i = 0; i < flagUsed.Length; i++)
        {
            flagUsed[i] = false;
        }

        // Deactivate all flags initially
        foreach (GameObject flag in flagObjects)
        {
            flag.SetActive(false);

            // Also disable colliders explicitly
            Collider[] colliders = flag.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }
        }

        // Start first trial
        StartNextTrial();
    }

    // Called from EditorController when spacebar is pressed
    public void PrepareNextTrial()
    {
        Debug.Log($"PrepareNextTrial called. trialInProgress: {trialInProgress}, experimentComplete: {experimentComplete}");

        if (experimentComplete)
        {
            Debug.Log("Experiment is already complete!");
            return;
        }

        if (!trialInProgress)
        {
            // Return player to starting position
            TeleportPlayerToSpawn();

            // Reset for next trial with a small delay
            StartCoroutine(DelayedNextTrial(1.0f));
        }
        else
        {
            Debug.Log("Cannot start next trial - current trial still in progress!");
        }
    }

    private IEnumerator DelayedNextTrial(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextTrial();
    }

    // Made public so EditorController can access it
    public void StartNextTrial()
    {
        // Reset DataLogger for new trial
        DataLogger.ResetForNextTrial();

        // Disable all flags first
        foreach (GameObject flag in flagObjects)
        {
            flag.SetActive(false);

            // Also disable colliders
            Collider[] colliders = flag.GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }
        }

        // Increment trial number ONLY ONCE
        currentTrialNumber++;
        Debug.Log($"Starting Trial #{currentTrialNumber}");

        // Create a list of available flags that haven't been used
        List<int> availableFlags = new List<int>();
        for (int i = 0; i < flagObjects.Length; i++)
        {
            if (!flagUsed[i])
            {
                availableFlags.Add(i);
            }
        }

        // Debug to see what's available
        Debug.Log($"Available flags: {string.Join(", ", availableFlags)}");

        // Check if we have any flags left
        if (availableFlags.Count == 0)
        {
            Debug.Log("All flags have been used. Experiment complete!");
            experimentComplete = true;
            return;
        }

        // Pick a random available flag
        int randomIndex = Random.Range(0, availableFlags.Count);
        int nextFlagIndex = availableFlags[randomIndex];
        currentFlagIndex = nextFlagIndex;

        // Activate only this flag
        flagObjects[nextFlagIndex].SetActive(true);

        // Enable colliders for the active flag
        Collider[] newFlagColliders = flagObjects[nextFlagIndex].GetComponentsInChildren<Collider>();
        foreach (Collider c in newFlagColliders)
        {
            c.enabled = true;
        }

        Debug.Log($"Trial {currentTrialNumber}: Flag #{nextFlagIndex} activated");

        // Mark this trial as in progress
        trialInProgress = true;
    }

    // Called by FlagTrigger when flag is reached
    public void FlagReached(GameObject hitFlag)
    {
        if (!trialInProgress)
        {
            Debug.Log("Flag hit, but no trial in progress!");
            return;
        }

        // Identify which flag was hit
        for (int i = 0; i < flagObjects.Length; i++)
        {
            if (flagObjects[i] == hitFlag)
            {
                Debug.Log($"Flag #{i} reached in Trial {currentTrialNumber}");

                // Mark flag as used in BOTH tracking systems
                usedFlagIndices.Add(i);
                flagUsed[i] = true;

                // Explicitly disable flag and its colliders
                Collider[] colliders = hitFlag.GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders)
                {
                    c.enabled = false;
                }
                hitFlag.SetActive(false);

                // Mark this trial as complete
                trialInProgress = false;

                // Prompt for spacebar press
                Debug.Log("Press SPACE to continue to next trial");
                break;
            }
        }
    }

    // Make public so it can be called from EditorController
    public void TeleportPlayerToSpawn()
    {
        if (playerTransform != null && playerSpawnPoint != null)
        {
            playerTransform.position = playerSpawnPoint.position;
            // playerTransform.rotation = playerSpawnPoint.rotation;
            Debug.Log("Player teleported to spawn point");
        }
        else
        {
            Debug.LogError("Cannot teleport player - missing references!");
            if (playerTransform == null) Debug.LogError("Player Transform is null!");
            if (playerSpawnPoint == null) Debug.LogError("Spawn Point is null!");
        }
    }
}
