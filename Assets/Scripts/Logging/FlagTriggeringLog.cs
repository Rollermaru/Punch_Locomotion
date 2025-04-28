using UnityEngine;
using Unity.Logging;

[RequireComponent(typeof(Collider))]
public class FlagTriggeringLog : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightHandAnchor"))
        {
            Log.Info("RightHandAnchor triggered flag: {0}", gameObject.name);
            GetComponent<Collider>().enabled = false;
        }
    }
}
