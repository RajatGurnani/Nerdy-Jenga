using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginUI : MonoBehaviour
{
    public InputField field;
    public TMP_Text outputText;
    Identification identification;
    public ConnectToServer connectToServer;

    private void Start()
    {
        identification = Identification.Instance;
    }

    public void UpdateUI()
    {
        if (field.text == "")
        {
            outputText.text = "Error: Invalid User Name";
            return;
        }

        outputText.text = "Logged in as " + field.text;
        identification.UpdateName(field.text);
        connectToServer.Connect();
    }

    public void ClearErrorOnType()
    {
        outputText.text = "";
    }
}