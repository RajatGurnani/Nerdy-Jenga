using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJenga : MonoBehaviourPunCallbacks
{
    public GameObject blockPrefab;
    public int height = 7;
    public Vector3 scale = new Vector3(3.2f, 0.8f, 1f);

    public void SpawnJengaBlocks()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < height; i++)
            {
                if (i % 2 == 0)
                {
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(0, 0.4f + 0.8f * i, 1.1f), Quaternion.identity);
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(0, 0.4f + 0.8f * i, 0f), Quaternion.identity);
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(0, 0.4f + 0.8f * i, -1.1f), Quaternion.identity);
                }
                else
                {
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(1.1f, 0.4f + 0.8f * i, 0), Quaternion.Euler(0, 90, 0));
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(0, 0.4f + 0.8f * i, 0f), Quaternion.Euler(0, 90, 0));
                    PhotonNetwork.Instantiate(blockPrefab.name, new Vector3(-1.1f, 0.4f + 0.8f * i, 0), Quaternion.Euler(0, 90, 0));
                }
            }
        }
    }
}
