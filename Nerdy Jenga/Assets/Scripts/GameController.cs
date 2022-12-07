using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    string playerName = "";
    public TMP_Text playerText;

    public void UpdatePlayerName(string _name)
    {
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
}
