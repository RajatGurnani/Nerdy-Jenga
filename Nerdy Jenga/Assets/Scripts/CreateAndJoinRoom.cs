using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    public string defaultRoomName = "Room";
    public const string KEY = "KEY";

    private void Start()
    {
        roomName.text = defaultRoomName;
        //roomName.text = "nerdy";
    }

    public void CreateRoom()
    {
        RoomOptions room = new RoomOptions();
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { KEY, Random.Range(0, 10000) } };
        PhotonNetwork.CreateRoom(roomName.text, room);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (message == "GameIdAlreadyExists" || returnCode == 32766)
        {
            JoinRoom();
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Game");
    }
}
