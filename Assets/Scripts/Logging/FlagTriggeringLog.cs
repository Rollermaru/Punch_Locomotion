using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagTrigger : MonoBehaviour
{
    private bool hasLogged = false;  // Ensure logging happens only once

    private void OnTriggerEnter(Collider other)
    {
        // If the collider that entered is your hand
        if (!hasLogged && other.gameObject.name.Contains("RightHandAnchor"))
        {
            hasLogged = true;
            // Call DataLogger to record the event
            DataLogger.LogFlagHit(gameObject.name, transform.position, other.transform.position );
            Debug.Log($"[Trigger] Flag '{name}' hit at time={Time.time:F2}");
        }
    }
}
