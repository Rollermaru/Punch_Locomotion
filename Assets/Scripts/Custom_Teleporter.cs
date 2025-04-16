using Oculus.Interaction.Locomotion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Teleporter : LocomotionEventsConnection
{
    public ActivateTP activate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public new void HandleLocomotionEvent(LocomotionEvent locomotionEvent)
    {

        if (activate) {
            Debug.Log("Handle Teleport");
            // Handler.HandleLocomotionEvent(locomotionEvent);
        }
        // Handler;
    }

    
}
