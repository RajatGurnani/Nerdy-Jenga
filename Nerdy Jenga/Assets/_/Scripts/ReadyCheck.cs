using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyCheck : MonoBehaviour
{
    public Player player;
    public string nickName;
    public bool readyInLobby = false;
    public Button readyPreGameButton;
    public Button questionAnswerButton;
    public GameObject readyCanvas;
    public PhotonView view;
    public GameController controller;
    public bool answered = false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        nickName = view.Owner.NickName;
    }

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        questionAnswerButton = controller.answerButton;
        readyCanvas = GameObject.FindGameObjectWithTag("ReadyCanvas");
        readyPreGameButton = GameObject.FindGameObjectWithTag("Ready").GetComponent<Button>();
        //questionAnswerButton = GameObject.FindGameObjectWithTag("ReadyQuestion").GetComponent<Button>();
        questionAnswerButton = controller.answerButton;
        readyPreGameButton.onClick.AddListener(() => Ready());
        questionAnswerButton.onClick.AddListener(() => Answered());
    }

    [PunRPC]
    public void SyncPlayerNames(int id)
    {
        if (id == view.ViewID)
        {
            nickName = PhotonNetwork.NickName;
        }
    }

    [PunRPC]
    public void EnablePlayer(int id)
    {
        if (id == view.ViewID)
        {
            nickName = PhotonNetwork.NickName;
            readyInLobby = true;
        }
    }

    [PunRPC]
    public void StartGameWithPlayers()
    {
        player.enabled = true;
        readyCanvas.SetActive(false);
    }

    public void Ready()
    {
        if (view.IsMine)
        {
            view.RPC(nameof(EnablePlayer), RpcTarget.AllBuffered, view.ViewID);
            readyPreGameButton.gameObject.SetActive(false);
        }
    }

    public void Answered()
    {
        if (view.IsMine)
        {
            view.RPC(nameof(AnswerRPC), RpcTarget.AllBuffered, view.ViewID);
        }
    }

    [PunRPC]
    public void AnswerRPC(int id)
    {
        if (id == view.ViewID)
        {
            answered = true;
            controller.QuestionAnswered();
        }
    }

    [PunRPC]
    public void ResetAnswer()
    {
        answered = false;
        questionAnswerButton.gameObject.SetActive(true);
    }
}