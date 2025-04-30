using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagProximityDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Dash dasher;

    private FlagManager flagManager;
    private bool hasTriggered;

    private void Start()
    {
        flagManager = FindObjectOfType<FlagManager>();
    }

    private void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        // Only register information once a dash finishes
        // And also if we're working in the dash scene
        if (dasher != null) {
            if (dasher.isDashing) return; 
        }
        
        HandleFlagTriggered();
    }

    private void HandleFlagTriggered()
    {
        hasTriggered = true;
        int currentTrial = flagManager?.CurrentTrialNumber ?? 0;

        // Log data through the DataLogger
        DataLogger.LogFlagHit(
            currentTrial,
            gameObject.name,
            transform.position,
            flagManager.PlayerTransform.position
        );

        // Notify FlagManager
        flagManager?.FlagReached(gameObject);

        // Disable the flag
        gameObject.SetActive(false);
    }

}
