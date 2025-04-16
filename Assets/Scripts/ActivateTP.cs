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
    [SerializeField] private int frames = 90;
    [SerializeField] private float dist_threshold = 0.3f;   // maybe 0.5 meters?
    private Queue<Vector3> controller_positions;
    public bool activatePunch = false;
    public Vector3 punch_direction; // to tell direction of punch for dashing in punch_direction's direction
    // Maybe have a vector that checks for forward punch? idk

    void Start()
    {
        controller_positions = new Queue<Vector3>(frames);
    }

    void Update()
    {
        Transform curr_transform = transform;
        AddPoint(curr_transform);
        activatePunch = CheckPunch(curr_transform);

        punch_direction = transform.position - controller_positions.Peek(); // then - now

        if (activatePunch) {
            Debug.Log("PUNCH!!!!");
            controller_positions.Clear();
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
        Vector3 then = controller_positions.Peek();
        Vector3 now = trans.position;

        if (now.magnitude - then.magnitude > dist_threshold) {
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
        if (controller_positions.Count >= frames) {
            controller_positions.Dequeue();
        }

        controller_positions.Enqueue(transform.position);
    }
}
