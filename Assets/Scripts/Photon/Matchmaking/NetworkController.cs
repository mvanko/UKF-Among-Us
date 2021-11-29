using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    private string server;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if(SceneManager.GetActiveScene().name == "Waiting Room")
        {

        }
    }

    public override void OnConnectedToMaster()
    {
        server = PhotonNetwork.CloudRegion;
        Debug.Log("Server " + server);
    }
}
