using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour
{
    public FlagManager flagManager;

    void Update()
    {
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
        }
    }
}
