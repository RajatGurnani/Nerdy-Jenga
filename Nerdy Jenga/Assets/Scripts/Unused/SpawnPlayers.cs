using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Material redTeam;
    public Material blueTeam;
    public TMP_InputField nameInput;
    public GameObject canvasUI;

    public void Start()
    {
        Vector3 rand = new Vector3(Random.Range(-10f, 10f), 2, Random.Range(-10f, 10f));
        //PhotonNetwork.Instantiate(playerPrefab.name, rand, Quaternion.identity);
    }
    public void Update()
    {
        Debug.Log(PhotonNetwork.GetPing());
    }

    /// <summary>
    /// Spawn player on Red Team
    /// </summary>
    public void SpawnToRedTeam()
    {
        GameObject temp = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-20, 2, 0), Quaternion.identity);
        temp.GetComponent<PlayerMovement>().playerTeam = PlayerMovement.TeamColor.Red;
        //temp.GetComponentInChildren<TMP_Text>().text = nameInput.text;
        canvasUI.SetActive(false);
    }

    /// <summary>
    /// Spawn player on Blue Team
    /// </summary>
    public void SpawnToBlueTeam()
    {
        GameObject temp = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(20, 2, 0), Quaternion.identity);
        temp.GetComponent<PlayerMovement>().playerTeam = PlayerMovement.TeamColor.Blue;
        //temp.GetComponentInChildren<TMP_Text>().text = nameInput.text;
        canvasUI.SetActive(false);
    }
}
