using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{
    public PhotonView view;
    public Camera cam;
    public GameController gameController;

    public static System.Action<string> BlocksFell;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        cam = Camera.main;
        Room room = PhotonNetwork.CurrentRoom;
        Debug.Log(room.CustomProperties["KEY"]);
        Debug.Log(PhotonNetwork.NickName);
        gameController = PhotonView.FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.CompareTag("Block"))
                {
                    int viewID = hit.collider.GetComponent<PhotonView>().ViewID;
                    view.RPC(nameof(DisableBlock), RpcTarget.AllBuffered, viewID);
                    view.RPC(nameof(GameOver), RpcTarget.AllBuffered, PhotonNetwork.NickName);
                }
            }
        }
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
