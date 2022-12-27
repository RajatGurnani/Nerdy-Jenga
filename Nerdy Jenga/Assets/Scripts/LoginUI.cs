using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LoginUI : MonoBehaviour
{
    public InputField field;
    public TMP_Text outputText;
    Identification identification;
    public ConnectToServer connectToServer;

    private void Start()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessfulLogin, OnError);
        identification = Identification.Instance;
    }

    public void OnSuccessfulLogin(LoginResult result)
    {

    }

    public void OnError(PlayFabError error)
    {

    }

    public void UpdateUI()
    {
        if (field.text == "")
        {
            outputText.text = "Error: Invalid User Name";
            return;
        }

        outputText.text = "Successfully logged in as " + field.text;
        identification.UpdateName(field.text);
        connectToServer.Connect();
    }

    public void ClearErrorOnType()
    {
        outputText.text = "";
    }
}