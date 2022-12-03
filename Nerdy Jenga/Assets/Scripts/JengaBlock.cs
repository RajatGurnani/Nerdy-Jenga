using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JengaBlock : MonoBehaviourPunCallbacks
{
    public Helper.BlockType blockType;
    public PhotonView view;
    public MeshRenderer meshRenderer;
    public Material familyMaterial;
    public Material workMaterial;
    public Material loveMaterial;
    public Material selfMaterial;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        blockType = (Helper.BlockType)Random.Range(0, 4);
        meshRenderer.material = SetMaterial(blockType);
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
