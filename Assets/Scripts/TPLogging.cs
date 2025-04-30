using Oculus.Interaction;
using Oculus.Interaction.PoseDetection.Debug;
using UnityEngine;

public class TPLogging : MonoBehaviour
{

    // For example timer
    private float elapsedTime = 0.0f;
    private float elapsedTime_mins = 0.0f;
    private float elapsedTime_secs = 0.0f;
    [SerializeField] private Axis2DActiveState activeState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool startedTimer = false;      // This bool is the important one. Tells when the first TP is taken
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If a teleport is broadcasted, start a timer
        if (activeState.Active) startedTimer = true;

        // This was used for debugging and can be discarded once the actual timer is put in
        if (startedTimer) {
            elapsedTime += Time.deltaTime;
            elapsedTime_mins = Mathf.FloorToInt(elapsedTime / 60);
            elapsedTime_secs = Mathf.FloorToInt(elapsedTime % 60);
        }
    }

    // Restart timer & variable that keeps track of timer when called (used in EditorController)
    public void PrepareNextTrial() {
        // Call any needed methods to prepare next trial (such as restarting time)
        startedTimer = false;
        elapsedTime = 0.0f;
    }


    // Was used for debugging
    public string getTimerText() {
        return string.Format("{0:00}:{1:00}", elapsedTime_mins, elapsedTime_secs);
    }
}
