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
        if (hasTriggered || !gameObject.activeInHierarchy) return;

        // Sphere cast for more accurate distance checking
        if (Physics.CheckSphere(transform.position, detectionRadius, playerLayer))
        {
            HandleFlagTriggered();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Skip if already triggered or if dash is in progress
        if (hasTriggered || !gameObject.activeInHierarchy) return;

        // Check if it's the hand or the camera rig
        bool isHand = other.name.Contains("RightHandAnchor");
        bool isPlayer = other.gameObject.layer == LayerMask.NameToLayer("Player") || 
            other.name.Contains("HandAnchor") || other.transform.root.name.Contains("Camera");

        if (!isHand && !isPlayer) return;

        // Skip if still dashing
        if (dasher != null && dasher.isDashing) return;

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
