using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField field;
    public TMP_Text outputText;
    Identification identification;
    public ConnectToServer connectToServer;
    public string id = "";
    public const string UNIQUE_ID = "UNIQUE_ID";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(UNIQUE_ID))
        {
            PlayerPrefs.SetString(UNIQUE_ID, System.Guid.NewGuid().ToString());
            PlayerPrefs.Save();
        }
        id = PlayerPrefs.GetString(UNIQUE_ID);
        Debug.Log(id);
    }

    private void Start()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessfulLogin, OnError);
        identification = Identification.Instance;
        identification.uniqueID = id;
    }

    public void OnSuccessfulLogin(LoginResult result) => Debug.Log("success- " + id);
    public void OnError(PlayFabError error) => Debug.Log("error- " + error.ToString());

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

    public void ClearErrorOnType() => outputText.text = "";
}