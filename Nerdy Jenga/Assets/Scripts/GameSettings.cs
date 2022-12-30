using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public Color color;
    private void Awake()
    {
        Debug.Log(ColorUtility.ToHtmlStringRGB(color));
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}