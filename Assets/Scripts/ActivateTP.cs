using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Logging;

// Place within a controller of your choosing
// Use the public bool activatePunch to initiate teleportation within dash script
public class ActivateTP : MonoBehaviour
{
    [Header("Punch Detection")]
    [SerializeField] private int frames = 24;
    [SerializeField] private float dist_threshold = 0.3f;   // maybe 0.5 meters?
    public Queue<Vector3> controller_positions;    // Punch activated by comparing first in queue to current position
    public Queue<Vector3> controllerPositionsLocal; // Local to parent
    public bool activatePunch;      // For other scripts to know that it's time to move

    public Vector3 punch_direction; // to tell direction of punch for dashing in punch_direction's direction

    public Dash dasher; // To get isDashing boolean to stop checking while moving
    private Transform savedPosition;
    public Transform floorPosition;

    [Header("Flag Proximity")]
    [SerializeField] private Transform[] flags;

    private bool[] hasLoggedFlag;
    public FlagManager flagManager;


    void Start()
    {
        controller_positions = new Queue<Vector3>(frames);
        controllerPositionsLocal = new Queue<Vector3>(frames);
        hasLoggedFlag = new bool[flags.Length];

        // Find the FlagManager in the scene
        flagManager = FindObjectOfType<FlagManager>();

        if (flagManager == null)
        {
            Debug.LogError("FlagManager not found in scene! Please add a FlagManager component.");
        }
    }

    void Update()
    {
        // Null checks
        if (dasher == null) return;
        

        // First handle punch detection
        if (dasher.isDashing) return;   // Don't do checks while dashing
        if (gameObject.transform.position.y < floorPosition.position.y) return; // Don't do checks under surface
        
        savedPosition = transform;
        AddPoint(savedPosition);
        activatePunch = CheckPunch(savedPosition);

        if (activatePunch)
        {
            punch_direction = Vector3.Normalize( savedPosition.position - controller_positions.Peek());
            controller_positions.Clear();
            controllerPositionsLocal.Clear();
            Debug.Log("PUNCH!!!!");
        }
        

        // If dashing is happening, Dash.cs will call DataLogger.LogTeleportStart()
        // So here, we only handle "flag hit" detection logic
        // for (int i = 0; i < flags.Length; i++)
        // {
        //     // Skip if already logged or if the flag is inactive
        //     if (hasLoggedFlag[i] || !flags[i].gameObject.activeInHierarchy) continue;

        //     float d = Vector3.Distance(transform.position, flags[i].position);

        //     if (d <= 5f)  // Within a 5-meter radius
        //     {
        //         hasLoggedFlag[i] = true;

        //         // Get the current trial number from FlagManager
        //         int currentTrial = 0;
        //         if (flagManager != null)
        //         {
        //             currentTrial = flagManager.CurrentTrialNumber;
        //         }

        //         // Log the event with the trial number
        //         DataLogger.LogFlagHit(currentTrial, flags[i].name, flags[i].position, transform.position);

        //         Debug.Log($"[Distance] Flag '{flags[i].name}' hit at t={Time.time:F2}, d={d:F2}, Trial={currentTrial}");

        //         if (flagManager != null)
        //             flagManager.FlagReached(flags[i].gameObject);
        //     }
        // }

    }

    /*
    *   Compares the distance between controller positions from Now & `frames` (90) frames ago
    *   Note, 0th index of `controller_positions` is the position of the controller `frames` (90) frames ago
    *
    *   @params trans
    *               Transformation of object at current frame
    *   @return true if distance >= dist_threshold (as given above)  
    */
    private bool CheckPunch(Transform trans) 
    {
        if (controller_positions.Count <= 0) return false;
        if (controllerPositionsLocal.Count <= 0) return false;

        Vector3 then = controllerPositionsLocal.Peek();
        Vector3 now = trans.localPosition;

        return (now - then).magnitude > dist_threshold;
    }

    /*
    *   On every frame, enqueue position of controller to end of Queue<Vector3> controller_positions.
    *   If array is full, dequeues first position then enqueues last position
    *
    *   @params trans
    *               Transformation of object at current frame
    */
    private void AddPoint(Transform trans) 
    {
        if (controller_positions.Count >= frames - 1) {
            controller_positions.Dequeue(); // O(1)?
        }

        if (controllerPositionsLocal.Count >= frames - 1) {
            controllerPositionsLocal.Dequeue();
        }

        controller_positions.Enqueue(trans.position);   // Also O(1) if queue has space
        controllerPositionsLocal.Enqueue(trans.localPosition);
    }
}
