using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float inputX = 0f;
    float inputY = 0f;
    public float moveSpeed = 10f;
    public Rigidbody rb;
    public PhotonView view;
    public TMP_Text nameText;
    string nameString = "";
    public Material red;
    public Material blue;
    public MeshRenderer renderer;

    public TeamColor playerTeam;

    public enum TeamColor
    {
        Red,
        Blue
    }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        nameText = GetComponentInChildren<TMP_Text>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            nameString = GameObject.FindObjectOfType<SpawnPlayers>().nameInput.text;
            view.RPC(nameof(SetText), RpcTarget.AllBuffered, nameString);
            view.RPC(nameof(SetMaterial), RpcTarget.AllBuffered, playerTeam);
        }
    }

    public void Update()
    {
        if (view.IsMine)
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
        }

        //transform.Translate(new Vector3(inputX, 0, inputY).normalized * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(inputX, 0, inputY).normalized * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 30);
    }


    /// <summary>
    /// Syncing player names across clients
    /// </summary>
    /// <param name="_name"></param>
    [PunRPC]
    public void SetText(string _name)
    {
        nameText.text = _name;
    }

    /// <summary>
    /// Syncing materials across clients
    /// </summary>
    /// <param name="color"></param>
    [PunRPC]
    public void SetMaterial(TeamColor color)
    {
        if(color == TeamColor.Red)
        {
            renderer.material = red;
        }
        else
        {
            renderer.material = blue;
        }
    }
}
