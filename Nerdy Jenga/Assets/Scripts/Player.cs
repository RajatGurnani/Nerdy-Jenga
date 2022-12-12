using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using UnityEngine.LowLevel;
using JetBrains.Annotations;

public class Player : MonoBehaviourPunCallbacks
{
    public PhotonView view;
    public Camera cam;
    public GameController gameController;
    public bool myturn = false;

    public static System.Action<string> BlocksFell;
    public string nickName;
    public StartGame startGame;
    public List<string> playerList = new();
    public Photon.Realtime.Player turnPlayer;

    private void Awake()
    {
        nickName = PhotonNetwork.NickName;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        startGame = PhotonView.FindObjectOfType<StartGame>();
        cam = Camera.main;
        gameController = PhotonView.FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.CompareTag("Block") && nickName == startGame.turnName)
                {
                    int viewID = hit.collider.GetComponent<PhotonView>().ViewID;
                    view.RPC(nameof(DisableBlock), RpcTarget.AllBuffered, viewID);
                    view.RPC(nameof(GameOver), RpcTarget.AllBuffered, PhotonNetwork.NickName);
                    myturn = false;
                    startGame.view.RPC(nameof(startGame.StartTurn), RpcTarget.AllBuffered);
                }
            }
        }
    }

    public void UpodateList()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            playerList.Add(item.NickName);
        }
        playerList = playerList.OrderBy(p => p).ToList();
    }

    [PunRPC]
    public void ChangeTurn()
    {
        startGame.turnText.text = turnPlayer.NickName;
    }

    [PunRPC]
    public void GameOver(string name)
    {
        BlocksFell?.Invoke(name);
    }

    [PunRPC]
    public void DisableBlock(int playerViewID)
    {
        PhotonView.Find(playerViewID).gameObject.SetActive(false);
    }
}
