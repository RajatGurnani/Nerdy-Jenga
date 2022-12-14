using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameController : MonoBehaviour
{
    string playerName = "";
    public TMP_Text playerText;
    public GameObject gameplayScreen;
    public TMP_Text questionText;
    public PhotonView view;
    public bool questionAsked = false;
    public List<Player> players=new();
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

    public void AskQuestion(string ques)
    {
        view.RPC(nameof(AskQuestionToAll), RpcTarget.AllBuffered, ques);
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
                questionText.text = "answered";
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
    public void AskQuestionToAll(string question)
    {
        players = FindObjectsOfType<Player>().ToList();
        questionAsked = true;
        gameplayScreen.SetActive(true);
        questionText.text = question;
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
        questionText.text = "";
    }
}
