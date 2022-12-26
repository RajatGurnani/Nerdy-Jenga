using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using UnityEngine.LowLevel;
using JetBrains.Annotations;
using UnityEngine.InputSystem.HID;

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

    public int temp = 0;
    public bool answered = false;

    public CommonQuestionsHolder questionsHolder;
    public Helper.BlockType blockTemp;
    public string quesTemp ="";

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
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && gameController.questionAsked == false)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                //if (hit.collider.CompareTag("Block") && nickName == (string)PhotonNetwork.CurrentRoom.CustomProperties["Turn"])
                if (hit.collider.CompareTag("Block") && nickName == startGame.turnName)
                {
                    Debug.Log("hitting");
                    int viewID = hit.collider.GetComponent<PhotonView>().ViewID;
                    view.RPC(nameof(DisableBlock), RpcTarget.AllBuffered, viewID);
                    view.RPC(nameof(GameOver), RpcTarget.AllBuffered, PhotonNetwork.NickName);
                    //startGame.view.RPC(nameof(startGame.StartTurn), RpcTarget.AllBuffered);
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
        blockTemp = _block;
        quesTemp = ques;
        Invoke(nameof(QuestionTemp), 1.5f);
        questionsHolder.view.RPC(nameof(questionsHolder.SyncValues), RpcTarget.AllBuffered, _block, temp);
    }

    public void QuestionTemp()
    {
        gameController.AskQuestion(blockTemp, quesTemp);
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
        JengaBlock temp=PhotonView.Find(playerViewID).gameObject.GetComponent<JengaBlock>();
        temp.animator.enabled = true;
        temp.audioSource.Play();
        Destroy(temp.gameObject,1f);
    }
}
