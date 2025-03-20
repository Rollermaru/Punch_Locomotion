using System.ComponentModel.Design;
using Oculus.Interaction.OVR.Input;
using Oculus.Interaction.PoseDetection.Debug;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    // Used to get positional data from controller & player
    public GameObject controller;
    public GameObject player;

    // Line rendering
    [SerializeField] LineRenderer lineRend;

    // Do different things depending on LayerMask aimed at
    public LayerMask enemies;
    public LayerMask terrain;

    // Refers to other scripts
    private MyActiveState myActiveState;
    public GameObject state;
    
    // Something
    public Transform origin;

    private bool enemyHit;
    private int raylen;
    private bool validSelection;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHit = false;
        validSelection = false;
        raylen = 10;
        myActiveState = state.GetComponent<MyActiveState>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!state.GetComponent<MyActiveState>().Active) {
            lineRend.enabled = false;
            return;
        }
        // Debug.Log("Pressing Trigger");
        

        var ray = new Ray(origin.position, controller.transform.forward);
        // Debug.Log(controller.transform.forward);

        RaycastHit hit; // if null, didn't hit anything
        Transform dest = null;
        

        lineRend.enabled = true;
        lineRend.SetPosition(0, origin.position);

        if (Physics.Raycast(ray, out hit, raylen, terrain)) // hits terrain
        {
            lineRend.SetPosition(1, hit.point);
            dest = hit.transform;
            validSelection = true;
        }

        else if (Physics.Raycast(ray, out hit, raylen, enemies)) // Hits enemies
        {
            lineRend.SetPosition(1, hit.point);
            dest = hit.transform;
            enemyHit = true;
            validSelection = true;
        } 
        else                                               // hits anything else (typically nothing)
        {
            lineRend.SetPosition(1, (origin.position + controller.transform.forward) * raylen); // why is this buggy?
                // in MyActiveState, controller.transform.forward tracks correctly...
            enemyHit = false;
            validSelection = false;
        }
        


        /* Swing Detected, begin teleportation */
        if (!state.GetComponent<MyActiveState>().Swing) return;

        Debug.Log("SWING");
        


        /* ----- TELEPORT ----- */
        // For all layers, teleport to selection
        // LERP to position within 0.5 seconds. Hopefully apply vingette
        if (validSelection) {
            Vector3 copy = dest.transform.position; // change to raycast position
            copy = copy + new Vector3(0.0f, player.transform.position.y + 2.5f, 0.0f);
            // player.transform.position = copy.position + new Vector3(0.0f, player.transform.position.y, 0.0f);
            float speed = 1.0f;
            var step = speed * Time.deltaTime;
            // player.transform.position = Vector3.MoveTowards(player.transform.position, copy.transform.position, step);  // why does this not lerp?
            // player.transform.position = dest.transform.position;
            // player.transform.position = Vector3.MoveTowards(player.transform.position, dest.transform.position, step);
            player.transform.position = copy;
        } 


        // If hitting an enemy, destroy enemy
        if (enemyHit == true) {
            Destroy(hit.transform.gameObject);
        }


    }

}
