using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagProximityDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    private FlagManager flagManager;
    private bool hasTriggered;

    private void Start()
    {
        flagManager = FindObjectOfType<FlagManager>();
        GetComponent<Collider>().isTrigger = true;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
