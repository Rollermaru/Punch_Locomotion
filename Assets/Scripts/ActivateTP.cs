using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

// Place within a controller of your choosing
// Use the public bool activatePunch to initiate teleportation within dash script
public class ActivateTP : MonoBehaviour
{
    [SerializeField] private int frames = 24;
    [SerializeField] private float dist_threshold = 0.3f;   // maybe 0.5 meters?
    public Queue<Vector3> controller_positions;    // Punch activated by comparing first in queue to current position
    public Queue<Vector3> controllerPositionsLocal; // Local to parent
    public bool activatePunch;      // For other scripts to know that it's time to move

    public Vector3 punch_direction; // to tell direction of punch for dashing in punch_direction's direction

    public Dash dasher; // To get isDashing boolean to stop checking while moving
    private Transform savedPosition;
    public Transform floorPosition;

    void Start()
    {
        controller_positions = new Queue<Vector3>(frames);
        controllerPositionsLocal = new Queue<Vector3>(frames);
    }

    void Update()
    {
        if (dasher.isDashing) return;   // Don't do checks whilst dashing
        if (gameObject.transform.position.y < floorPosition.position.y) return; // Don't do checks under surface

        savedPosition = transform;
        AddPoint(savedPosition);
        activatePunch = CheckPunch(savedPosition);

        if (activatePunch) {
            Debug.Log("PUNCH!!!!");
            punch_direction = Vector3.Normalize(savedPosition.position - controller_positions.Peek()); // now - then
            controller_positions.Clear();   // O(n)
            controllerPositionsLocal.Clear();
        }

        
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

        if (Mathf.Abs((now - then).magnitude) > dist_threshold) {
            return true;
        }

        return false;
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

    public Vector3[] GetPoints() {
        Vector3[] points = {Vector3.zero, Vector3.zero};
        if (controller_positions.Count > 0) {
            points = new Vector3[] {controller_positions.Peek(), savedPosition.position};
        }

        return points;
    }
}
