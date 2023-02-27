using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class SendTime : MonoBehaviour
{
    public StartGame startGame;
    public HotelInformation hotelInfo;

    [Header("Hotel Custom Text")]
    public TMP_Text messageText;
    public string drink;
    public string drinkMessage;
    public string message;

    private void OnEnable()
    {
        messageText.text = hotelInfo.GiveMessage();
        //ChangeHotelInfo();
        if (PhotonNetwork.IsMasterClient)
        {
            Send();
        }
    }
    private void Start()
    {
        PhotonNetwork.Disconnect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            messageText.text = hotelInfo.GiveMessage();
        }
    }

    public void ChangeHotelInfo()
    {
        drink = $"<color=#FFF317>{hotelInfo.drinks[Random.Range(0, hotelInfo.drinks.Count)]}</color>";
        drinkMessage = hotelInfo.drinkMessage[Random.Range(0, hotelInfo.drinkMessage.Count)];

        message = string.Format(drinkMessage, drink);
        messageText.text = message;
    }

    public void Send()
    {
        int timePassed = (int)PhotonNetwork.Time - (int)PhotonNetwork.CurrentRoom.CustomProperties["startTime"];
        WriteTitleEventRequest request = new WriteTitleEventRequest()
        {
            Body = new Dictionary<string, object>()
            {
            { "Hotel_Name_Time",timePassed}
            },
            EventName = "Lobby_Time"
        };
        PlayFabClientAPI.WriteTitleEvent(request, success => Debug.Log("success"), error => Debug.Log("error"));
        Debug.Log(timePassed);
    }


    public void OpenURL()
    {
        Application.OpenURL(hotelInfo.url);
    }

    public void OpenReviewURL()
    {
        Application.OpenURL(hotelInfo.reviewUrl);
    }
}
