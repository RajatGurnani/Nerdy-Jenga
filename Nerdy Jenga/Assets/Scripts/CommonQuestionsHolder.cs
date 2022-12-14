using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CommonQuestionsHolder : MonoBehaviour
{
    public Questions familyQuestions;
    public Questions workQuestions;
    public Questions selfQuestions;
    public Questions loveQuestions;

    public List<int> familyList = new();
    public List<int> workList = new();
    public List<int> selfList = new();
    public List<int> loveList = new();
    public PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SyncValues(Helper.BlockType block, int value)
    {
        switch (block)
        {
            case Helper.BlockType.Family:
                familyList.Add(value);
                break;
            case Helper.BlockType.Work:
                workList.Add(value);
                break;
            case Helper.BlockType.Self:
                selfList.Add(value);
                break;
            case Helper.BlockType.Love:
                loveList.Add(value);
                break;
        }
    }
}
