using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour
{
    public FlagManager flagManager;
    public TPLogging TPLogger;

    void Update()
    {
        OVRInput.Update();
        // This will only be processed by the computer running Unity
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("Spacebar pressed by experimenter - moving to next trial");
            if (flagManager != null)
            {
                // Use PrepareNextTrial which handles both teleporting and starting next trial
                flagManager.PrepareNextTrial();
            }
            else
            {
                Debug.LogError("FlagManager reference is missing!");
            }

            if (TPLogger != null) {
                TPLogger.PrepareNextTrial();
            }
            else {
                Debug.Log("Not doing the TP trial, right?");
            }
        }
    }
}
