using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JengaBlock : MonoBehaviourPunCallbacks
{
    public bool audioPlayable = false;
    public Helper.BlockType blockType;
    public PhotonView view;
    public MeshRenderer meshRenderer;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip endClip;

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
    public bool gameOver = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        //meshRenderer = GetComponent<MeshRenderer>();
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
            Invoke(nameof(InvokeDelayEndGame), 1f);
        }
    }

    public void InvokeDelayEndGame()
    {
        view.RPC(nameof(EndGame), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void EndGame()
    {
        if (audioPlayable)
        {
            audioPlayable = false;
            Debug.Log("audio playing");
            audioSource.PlayOneShot(endClip, 0.1f);
        }
        boom.transform.GetChild(4).gameObject.SetActive(true);
        //Time.timeScale = 0f;
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
