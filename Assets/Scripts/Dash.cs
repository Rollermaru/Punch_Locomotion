using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 5f;      // How far the character will dash
    public float dashDuration = 0.2f;    // How long the dash takes (in seconds)

    public Transform playerOrigin;      // Object to be moved
    // public Transform playerHead;
    public ActivateTP punched;          // Reference to a boolean that tells us to activate the punch
    public Transform floorPosition;     // Need to know floor position to not go through it

    public bool isDashing;              // Tracks whether a dash is currently in progress
    private Vector3 startPos, endPos;    // Dash start and target positions
    private float timer;                 // Keeps track of time since the dash started
    [SerializeField] private FlagManager flagManager;


    void Start()
    {
        isDashing = false;
    }

    void LateUpdate()
    {
        if (punched.activatePunch && !isDashing)
        {
            StartDash();  // Begin dash movement
            punched.activatePunch = false;
        }

        // If currently dashing, update the character's position using linear interpolation
        if (isDashing)
        {
            timer += Time.deltaTime;
            float t = timer / dashDuration;  // Normalized time (0 to 1)
            playerOrigin.position = Vector3.Lerp(startPos, endPos, t);  // Smooth movement

            // End the dash once the duration is complete
            if (t >= 1f)
            {
                isDashing = false;
            }
        }
    }

    // Starts the dash movement by setting the starting position,
    // target position, and resetting the timer.
    void StartDash()
    {
        // Get current trial number from FlagManager
        int currentTrial = flagManager.CurrentTrialNumber;

        // Log teleport start with trial number
        DataLogger.LogTeleportStart(currentTrial);

        isDashing = true;
        timer = 0f;
        startPos = playerOrigin.position;
        endPos = startPos + punched.punch_direction * dashDistance;  // Dash in the punch direction

        if (endPos.y < floorPosition.position.y)
        {
            endPos.y = floorPosition.position.y;
        }
    }

}