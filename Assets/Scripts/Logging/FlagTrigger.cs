using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagTrigger : MonoBehaviour
{
    private FlagManager flagManager;
    private bool hasTriggered = false;

    private void Start()
    {
        // Find the flag manager in the scene
        flagManager = FindObjectOfType<FlagManager>();
        if (flagManager == null)
        {
            Debug.LogError("FlagManager not found in scene!");
        }
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
            FlagManager flagManager = FindObjectOfType<FlagManager>();
            if (flagManager != null)
            {
                // call FlagReached with this GameObject
                flagManager.FlagReached(this.gameObject);
                Debug.Log($"Flag '{name}' triggered FlagManager.FlagReached");
            }
            else
            {
                Debug.LogError("FlagManager not found!");
            }
        }
    }

}
