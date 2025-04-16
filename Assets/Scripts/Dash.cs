using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 5f;      // How far the character will dash
    public float dashDuration = 0.2f;    // How long the dash takes (in seconds)

    private bool isDashing;              // Tracks whether a dash is currently in progress
    private Vector3 startPos, endPos;    // Dash start and target positions
    private float timer;                 // Keeps track of time since the dash started

    void Update()
    {
        // Detect if the Space key was pressed this frame
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isDashing)
        {
            StartDash();  // Begin dash movement
        }

        // If currently dashing, update the character's position using linear interpolation
        if (isDashing)
        {
            timer += Time.deltaTime;
            float t = timer / dashDuration;  // Normalized time (0 to 1)
            transform.position = Vector3.Lerp(startPos, endPos, t);  // Smooth movement

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
        isDashing = true;
        timer = 0f;
        startPos = transform.position;
        endPos = startPos + transform.forward * dashDistance;  // Dash in the forward direction
    }
}