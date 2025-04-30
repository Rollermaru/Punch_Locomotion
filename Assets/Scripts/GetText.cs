using UnityEngine;
using TMPro;

public class GetText : MonoBehaviour
{
    [SerializeField] private TPLogging timer;
    [SerializeField] private TextMeshPro timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        timerText.text = timer.getTimerText();
    }
}
