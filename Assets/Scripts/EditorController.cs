using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour
{
    public FlagManager flagManager;
    public TPLogging TPLogger;
    public ManageScene SceneManagementer;   // weird name because "SceneManager" already exists

    void Update()
    {
        OVRInput.Update();

        // Restart for next trial
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.Get(OVRInput.Button.Four))
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

        // Load next scene
        
        if (Input.GetKeyDown(KeyCode.Backspace) || OVRInput.Get(OVRInput.Button.Start)) {
            Debug.Log("Load?");
            if (SceneManagementer != null) {
                Debug.Log("Next Scene");
                SceneManagementer.GoToNextScene();

            }
        }
    }
}
