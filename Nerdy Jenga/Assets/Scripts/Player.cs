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

    public static System.Action<string> BlocksFell;
    public string nickName;
    public StartGame startGame;
    public List<string> playerList = new();
    public Photon.Realtime.Player turnPlayer;

    public Questions familyQuestions;
    public Questions workQuestions;
    public Questions selfQuestions;
    public Questions loveQuestions;

    public List<int> familyList = new();
    public List<int> workList = new();
    public List<int> selfList = new();
    public List<int> loveList = new();

    public int temp = 0;
    public bool answered = false;

    public CommonQuestionsHolder questionsHolder;

    private void Awake()
    {
        nickName = PhotonNetwork.NickName;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        questionsHolder = FindObjectOfType<CommonQuestionsHolder>();
        startGame = PhotonView.FindObjectOfType<StartGame>();
        cam = Camera.main;
        gameController = PhotonView.FindObjectOfType<GameController>();
    }

    private void Update()
    {
        Debug.Log(answered);
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && gameController.questionAsked == false)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.CompareTag("Block") && nickName == startGame.turnName)
                {
                    int viewID = hit.collider.GetComponent<PhotonView>().ViewID;
                    view.RPC(nameof(DisableBlock), RpcTarget.AllBuffered, viewID);
                    view.RPC(nameof(GameOver), RpcTarget.AllBuffered, PhotonNetwork.NickName);
                    startGame.view.RPC(nameof(startGame.StartTurn), RpcTarget.AllBuffered);
                    AskQuestion(hit.collider.GetComponent<JengaBlock>().blockType);
                }
            }
        }
    }

    public void AskQuestion(Helper.BlockType _block)
    {
        temp = Random.Range(0, questionsHolder.familyQuestions.question.Count);
        string ques = string.Empty;
        switch (_block)
        {
            case Helper.BlockType.Family:
                while (questionsHolder.familyList.Contains(temp))
                {
                    int count = questionsHolder.familyQuestions.question.Count;
                    temp = Random.Range(0, count);
                }
                ques = questionsHolder.familyQuestions.question[temp];
                break;

            case Helper.BlockType.Work:
                while (questionsHolder.workList.Contains(temp))
                {
                    int count = questionsHolder.workQuestions.question.Count;
                    temp = Random.Range(0, count);
                }
                ques = questionsHolder.workQuestions.question[temp];
                break;

            case Helper.BlockType.Love:
                while (questionsHolder.loveList.Contains(temp))
                {
                    int count = questionsHolder.loveQuestions.question.Count;
                    temp = Random.Range(0, count);
                }
                ques = questionsHolder.loveQuestions.question[temp];
                break;

            case Helper.BlockType.Self:
                while (questionsHolder.selfList.Contains(temp))
                {
                    int count = questionsHolder.selfQuestions.question.Count;
                    temp = Random.Range(0, count);
                }
                ques = questionsHolder.selfQuestions.question[temp];
                break;
        }
        gameController.AskQuestion(ques);
        questionsHolder.view.RPC(nameof(questionsHolder.SyncValues), RpcTarget.AllBuffered, _block, temp);
    }

    [PunRPC]
    public void UpdateFamily(int _temp)
    {
        familyList.Add(_temp);
    }

    [PunRPC]
    public void UpdateWork(int _temp)
    {
        workList.Add(_temp);
    }

    [PunRPC]
    public void UpdateLove(int _temp)
    {
        loveList.Add(_temp);
    }

    [PunRPC]
    public void UpdateSelf(int _temp)
    {
        selfList.Add(_temp);
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
