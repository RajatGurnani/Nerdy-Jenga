using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

public class JengaBlock : MonoBehaviourPunCallbacks
{
    public Helper.BlockType blockType;
    public PhotonView view;
    public MeshRenderer meshRenderer;
    public Material familyMaterial;
    public Material workMaterial;
    public Material loveMaterial;
    public Material selfMaterial;
    public int value;
    int roomValue = 0;
    public Rigidbody rb;
    public float fallValue = 1;
    public bool fall = false;
    public GameObject boom;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();
        roomValue = (int)PhotonNetwork.CurrentRoom.CustomProperties["KEY"];
    }

    private void Start()
    {
        boom = PhotonView.FindObjectOfType<EndGame>().gameObject;
        value = (roomValue % value) % 4;
        blockType = (Helper.BlockType)value;
        meshRenderer.material = SetMaterial(blockType);
    }

    private void Update()
    {
        if (rb.velocity.magnitude >= fallValue)
        {
            view.RPC(nameof(EndGame), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void EndGame()
    {
        boom.transform.GetChild(0).gameObject.SetActive(true);
    }

    public Material SetMaterial(Helper.BlockType blockType = Helper.BlockType.Family)
    {
        switch (blockType)
        {
            case Helper.BlockType.Family:
                return familyMaterial;
            case Helper.BlockType.Work:
                return workMaterial;
            case Helper.BlockType.Love:
                return loveMaterial;
            case Helper.BlockType.Self:
                return selfMaterial;
        }
        return familyMaterial;
    }
}
