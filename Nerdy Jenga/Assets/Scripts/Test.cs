using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Test : MonoBehaviour
{
    public TMP_InputField field;
    public TMP_Text outputText;

    void Start()
    {
        WebGLInput.mobileKeyboardSupport = true;
    }

    public void UpdateUI()
    {
        outputText.text = field.text;
    }
}
