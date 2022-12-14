using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class StartGame : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TMP_Text playerNamesText;
    public List<Photon.Realtime.Player> players = new List<Photon.Realtime.Player>();
    public List<string> names = new List<string>();
    public PhotonView view;
    public int playerCount = 0;
    public GameObject startButton;
    public GameObject spawnManager;
    public GameObject startScreen;
    public Queue<Player> playerScripts;
    public TMP_Text turnText;
    public Player[] playersArr;

    public string turnName;
    public Queue<string> playerNames;

    public bool gameStarted = false;
    public List<string> playerList;
    public Queue<string> playerQueue = new();
    public int turnNumber = 0;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        turnText = GameObject.FindGameObjectWithTag("Turn").GetComponent<TMP_Text>();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //view.RPC(nameof(ConnectNewPlayer), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ConnectNewPlayer()
    {
        players = PhotonNetwork.PlayerList.ToList();

        names = new();
        playerNamesText.text = "";

        foreach (var item in players)
        {
            names.Add(item.NickName);
            playerNamesText.text += $"\u2022<indent=1em> {item.NickName} </indent>\n";
        }
    }

    private void Update()
    {
        int length = PhotonNetwork.PlayerList.Length;
        if (playerCount != PhotonNetwork.PlayerList.Length)
        {
            ConnectNewPlayer();
            playerCount = length;
        }
    }

    public void StartPlay()
    {
        if (view.IsMine && PhotonNetwork.IsMasterClient)
        {
            Photon.Realtime.Player[] pList = PhotonNetwork.PlayerList;
            view.RPC(nameof(SetSpawnManager), RpcTarget.AllBuffered);
        }
    }

    public void UpdatePlayerList()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            playerList.Add(item.NickName);
        }

        playerList = playerList.OrderBy(p => p).ToList();
        playerQueue = new(playerList);
        Debug.Log(playerList.Count);
    }

    [PunRPC]
    public void SetSpawnManager()
    {
        UpdatePlayerList();
        playerQueue = new(playerList);
        spawnManager.SetActive(true);
        startScreen.SetActive(false);
        view.RPC(nameof(StartTurn), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StartTurn()
    {
        string temp = playerQueue.Dequeue();
        playerQueue.Enqueue(temp);
        turnName = temp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Turn", temp } });
        turnText.text = $"{temp}'s turn";
    }
}
