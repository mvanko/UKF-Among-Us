using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickJoinConroller : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject quickStartButton; 

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    public void QuickStart()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room, try creating one!");
    }
}
