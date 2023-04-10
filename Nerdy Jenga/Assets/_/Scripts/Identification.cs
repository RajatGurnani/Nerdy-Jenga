using UnityEngine;

public class Identification : MonoBehaviour
{
    public string uniqueID = "";
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

    private void OnGUI()
    {
        GUIStyle style = GUI.skin.box;
        style.fontSize = 30;
        GUI.Button(new Rect(400, 100, 700, 100), $"{uniqueID}", style);
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
