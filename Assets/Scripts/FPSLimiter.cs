 using UnityEngine;

public class FPSLimiter : MonoBehaviour
{

    [SerializeField] private int FPS = 60;

    void Start() 
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS;
    }

} 