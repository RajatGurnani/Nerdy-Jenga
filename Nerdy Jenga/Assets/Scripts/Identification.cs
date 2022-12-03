using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identification : MonoBehaviour
{
    public string userName;
    public static Identification Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //WebGLInput.mobileKeyboardSupport = true;
    }

    public void UpdateName(string _name)
    {
        userName = _name;
    }
}
