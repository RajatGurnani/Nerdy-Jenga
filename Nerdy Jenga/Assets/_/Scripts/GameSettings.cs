using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}