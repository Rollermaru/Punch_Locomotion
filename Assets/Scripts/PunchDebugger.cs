using System.Collections;
using UnityEngine;


/**
 *  This script didn't work out lol
 *  Wouldn't render a line from startPoint to endPoint on every punch
 */
public class PunchDebugger : MonoBehaviour
{

    public Dash dashScript;
    public ActivateTP tpScript;
    public LineRenderer punchLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        punchLine.useWorldSpace = true;
        punchLine.enabled = true;
        punchLine.endColor = new Color(0, 1, 1, 1);
        punchLine.startColor = new Color(1, 1, 1, 1);
        punchLine.startWidth = 0.2f;
        punchLine.endWidth = 0.2f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (tpScript.activatePunch) {
            Vector3[] startEndPoints = tpScript.GetPoints();
            punchLine.SetPositions(startEndPoints);
        }
    }
}
