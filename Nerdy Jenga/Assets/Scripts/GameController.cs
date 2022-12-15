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
    public GameObject gameplayScreen;
    public PhotonView view;
    public bool questionAsked = false;
    public List<Player> players=new();

    [Header("Question Panel")]
    public Image questionBG;
    public TMP_Text questionTypeText;
    public TMP_Text questionDescriptionText;
    public Sprite familySprite;
    public Sprite loveSprite;
    public Sprite workSprite;
    public Sprite selfSprite;

    public Button answerButton;
    public TMP_Text readyText;
    public StartGame startGame;

    private void Start()
    {
        startGame=FindObjectOfType<StartGame>();
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

    public void AskQuestion(Helper.BlockType _block,string ques)
    {
        view.RPC(nameof(AskQuestionToAll), RpcTarget.AllBuffered,_block,ques);
        //gameplayScreen.SetActive(true);
        //questionText.text = ques;
    }

    public void StopQuestion()
    {
        questionAsked = false;
        foreach (Player player in players)
        {
            Debug.Log("in loop");
            if (player.nickName == PhotonNetwork.NickName)
            {
                questionDescriptionText.text = "answered";
                player.answered= true;
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
        view.RPC(nameof(StopQuestionToAll), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void AskQuestionToAll(Helper.BlockType _block,string question)
    {
        questionTypeText.text = _block.ToString();
        switch (_block)
        {
            case Helper.BlockType.Family:
                questionBG.sprite = familySprite;
                break;
            case Helper.BlockType.Work:
                questionBG.sprite = workSprite;
                break;
            case Helper.BlockType.Love:
                questionBG.sprite = loveSprite;
                break;
            case Helper.BlockType.Self:
                questionBG.sprite = selfSprite;
                break;
            default:
                break;
        }
        players = FindObjectsOfType<Player>().ToList();
        questionAsked = true;
        gameplayScreen.SetActive(true);
        questionDescriptionText.text = question;
        QuestionAnswered();
        startGame.turnText.gameObject.SetActive(false);
    }

    [PunRPC]
    public void StopQuestionToAll()
    {
        readyText.text = "";
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            //player.answered= false;
        }
        questionAsked = false;
        gameplayScreen.SetActive(false);
        questionDescriptionText.text = "";
        answerButton.gameObject.SetActive(true);

        ReadyCheck[] readyChecks = FindObjectsOfType<ReadyCheck>();
        foreach (var check in readyChecks)
        {
            check.view.RPC(nameof(check.ResetAnswer),RpcTarget.AllBuffered);
        }
        startGame.turnText.gameObject.SetActive(true);
    }
}
