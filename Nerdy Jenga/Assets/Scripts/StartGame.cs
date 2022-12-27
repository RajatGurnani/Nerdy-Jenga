using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;

public class StartGame : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TMP_Text roomNameText;

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
    List<ReadyCheck> readyChecks = new();
    public string turnName;
    public string nextTurnName;
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

    private void Start()
    {
        roomNameText.text = $"Room Name: {PhotonNetwork.CurrentRoom.Name}";
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

        //foreach (var item in players)
        //{
        //    names.Add(item.NickName);
        //    playerNamesText.text += $"\u2022<indent=1em> {item.NickName} </indent>\n";
        //}

        foreach (var check in readyChecks)
        {
            if (check.readyInLobby)
            {
                //playerNamesText.text += $"\u2022<indent=1em> <color=green>{check.view.Owner.NickName}</color> </indent>\n";
                playerNamesText.text += $"<sprite name=\"tick\"> {check.view.Owner.NickName}\n";
            }
            else
            {
                //playerNamesText.text += $"\u2022<indent=1em> <color=red>{check.view.Owner.NickName}</color> </indent>\n";
                playerNamesText.text += $"<sprite name=\"cross\"> {check.view.Owner.NickName}\n";
            }
        }
    }

    private void Update()
    {
        int length = PhotonNetwork.PlayerList.Length;
        if (playerCount != PhotonNetwork.PlayerList.Length)
        {
            view.RPC(nameof(ConnectNewPlayer), RpcTarget.AllBuffered);
            ConnectNewPlayer();
            playerCount = length;
        }

        ConnectNewPlayer();
        readyChecks = new(PhotonView.FindObjectsOfType<ReadyCheck>());
        if (readyChecks.All(x => (x.readyInLobby == true)) && (gameStarted == false) && PhotonNetwork.IsMasterClient)
        {
            gameStarted = true;
            StartPlay();
        }
    }

    public void StartPlay()
    {
        if (view.IsMine && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.CustomProperties["startTime"] = (int)PhotonNetwork.Time;
            gameStarted = true;
            Photon.Realtime.Player[] pList = PhotonNetwork.PlayerList;
            RegisterLobby(pList.Length);
            view.RPC(nameof(SetSpawnManager), RpcTarget.AllBuffered);
        }
    }

    public void RegisterLobby(int count)
    {
        WriteTitleEventRequest request = new WriteTitleEventRequest()
        {
            Body = new Dictionary<string, object>()
            {
            { "Hotel_Name",count}
            },
            EventName = "Table_Size"
        };
        PlayFabClientAPI.WriteTitleEvent(request, success => Debug.Log("success"), error => Debug.Log("error"));
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
        gameStarted = true;
        UpdatePlayerList();
        playerQueue = new(playerList);
        spawnManager.SetActive(true);
        foreach (var check in readyChecks)
        {
            check.view.RPC(nameof(check.StartGameWithPlayers), RpcTarget.AllBuffered);
        }
        startScreen.SetActive(false);
        view.RPC(nameof(StartTurn), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StartTurn()
    {
        turnName = playerQueue.Dequeue();
        playerQueue.Enqueue(turnName);
        nextTurnName=playerQueue.Peek();
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Turn", turnName } });
        turnText.text = $"{turnName}'s turn";
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        playerQueue = new Queue<string>(playerQueue.Where(x => x != otherPlayer.NickName));
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["Turn"] == otherPlayer.NickName)
        {
            view.RPC(nameof(StartTurn), RpcTarget.AllBuffered);
        }
    }

    public void ReturnToMenu()
    {
        view.RPC(nameof(KickAll), RpcTarget.AllBuffered);
    }


    [PunRPC]
    public void KickAll()
    {
        Debug.Log("Exit");
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }
}
