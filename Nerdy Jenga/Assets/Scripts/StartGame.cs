using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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
    List<ReadyCheck> readyChecks = new();
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

        //foreach (var item in players)
        //{
        //    names.Add(item.NickName);
        //    playerNamesText.text += $"\u2022<indent=1em> {item.NickName} </indent>\n";
        //}

        foreach (var check in readyChecks)
        {
            if (check.readyInLobby)
            {
                playerNamesText.text += $"\u2022<indent=1em> <color=green>{check.view.Owner.NickName}</color> </indent>\n";
            }
            else
            {
                playerNamesText.text += $"\u2022<indent=1em> <color=red>{check.view.Owner.NickName}</color> </indent>\n";
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
            gameStarted = true;
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
        string temp = playerQueue.Dequeue();
        Debug.Log(temp + "-" + playerQueue.Count);
        playerQueue.Enqueue(temp);
        Debug.Log(temp + "-" + playerQueue.Count);
        turnName = temp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Turn", temp } });
        turnText.text = $"{temp}'s turn";
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
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
