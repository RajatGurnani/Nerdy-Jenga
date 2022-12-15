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
    }

    [PunRPC]
    public void StopQuestionToAll()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            //player.answered= false;
        }
        questionAsked = false;
        gameplayScreen.SetActive(false);
        questionDescriptionText.text = "";
    }
}
