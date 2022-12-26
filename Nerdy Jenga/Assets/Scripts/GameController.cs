using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    string playerName = "";
    public TMP_Text playerText;
    public GameObject questionScreen;
    public GameObject questionScreenSpectator;
    public GameObject questionParticipantsUI;
    public TMP_Text turnText;
    public TMP_Text spectatorText;

    public PhotonView view;
    public bool questionAsked = false;
    public List<Player> players = new();

    [Header("Question Panel")]
    public Image questionBG;
    public Image spectatorBG;
    public TMP_Text questionTypeText;
    public TMP_Text questionDescriptionText;

    public Sprite familySprite;
    public Sprite loveSprite;
    public Sprite workSprite;
    public Sprite selfSprite;

    public AudioSource questionAudio;

    public Button answerButton;
    public TMP_Text readyText;
    public StartGame startGame;

    private void Start()
    {
        startGame = FindObjectOfType<StartGame>();
        //questionScreen = GameObject.FindGameObjectWithTag("QScreen").transform.GetChild(0).gameObject;
    }

    public void UpdatePlayerName(string _name)
    {
        view = GetComponent<PhotonView>();
        playerName = _name;
        playerText.text = playerName;
    }

    private void OnEnable()
    {
        Player.BlocksFell += UpdatePlayerName;
    }

    private void OnDisable()
    {
        Player.BlocksFell -= UpdatePlayerName;
    }

    public void AskQuestion(Helper.BlockType _block, string ques)
    {
        view.RPC(nameof(AskQuestionToAll), RpcTarget.AllBuffered, _block, ques);
    }

    public void StopQuestion()
    {
        questionAsked = false;
        foreach (Player player in players)
        {
            if (player.nickName == PhotonNetwork.NickName)
            {
                questionDescriptionText.text = "answered";
                player.answered = true;
            }
            if (player.answered == false)
            {
                Debug.Log("whoops");
                return;
            }
        }
        view.RPC(nameof(StopQuestionToAll), RpcTarget.AllBuffered);
    }

    public void QuestionAnswered()
    {
        ReadyCheck[] readyChecks = FindObjectsOfType<ReadyCheck>();
        readyText.text = "";

        foreach (var check in readyChecks)
        {
            if (check.answered)
            {
                readyText.text += $"\u2022<indent=1em> <color=green>{check.view.Owner.NickName}</color> </indent>\n";
            }
            else
            {
                readyText.text += $"\u2022<indent=1em> <color=red>{check.view.Owner.NickName}</color> </indent>\n";
            }
        }
        foreach (var check in readyChecks)
        {
            if (check.answered == false)
            {
                return;
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startGame.view.RPC(nameof(startGame.StartTurn), RpcTarget.AllBuffered);
        }
        view.RPC(nameof(StopQuestionToAll), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void AskQuestionToAll(Helper.BlockType _block, string question)
    {
        questionAudio.Play();
        questionTypeText.text = _block.ToString();
        Sprite sprite = familySprite;
        switch (_block)
        {
            case Helper.BlockType.Family:
                sprite = familySprite;
                break;
            case Helper.BlockType.Work:
                sprite = workSprite;
                break;
            case Helper.BlockType.Love:
                sprite = loveSprite;
                break;
            case Helper.BlockType.Self:
                sprite = selfSprite;
                break;
            default:
                break;
        }
        questionBG.sprite = sprite;
        spectatorBG.sprite = sprite;
        players = FindObjectsOfType<Player>().ToList();

        if (PhotonNetwork.NickName == startGame.turnName || PhotonNetwork.NickName == startGame.nextTurnName)
        {
            questionDescriptionText.text = question;
            questionScreen.SetActive(true);
        }
        else
        {
            questionScreenSpectator.SetActive(true);
        }

        turnText.text = $"{startGame.turnName} <color=#69ED67>asks</color> {startGame.nextTurnName}";
        spectatorText.text = $"{startGame.turnName} <color=#FF7F7F>asks</color> {startGame.nextTurnName}";
        questionParticipantsUI.SetActive(true);
        questionAsked = true;
        QuestionAnswered();
        startGame.turnText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void StopQuestionToAll()
    {
        readyText.text = "";

        //Player[] players = FindObjectsOfType<Player>();
        //foreach (Player player in players)
        //{
        //    //player.answered= false;
        //}

        questionAsked = false;
        questionScreen.SetActive(false);
        questionParticipantsUI.SetActive(false);
        questionScreenSpectator.SetActive(false);
        questionDescriptionText.text = "";
        answerButton.gameObject.SetActive(true);

        ReadyCheck[] readyChecks = FindObjectsOfType<ReadyCheck>();
        foreach (var check in readyChecks)
        {
            check.view.RPC(nameof(check.ResetAnswer), RpcTarget.AllBuffered);
        }
        startGame.turnText.gameObject.SetActive(true);
    }
}
