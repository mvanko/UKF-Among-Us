using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] Text _uiServerText;

    private string server;

    public event Action OnConnectedToServer;

    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.ConnectingToMasterServer)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        OnConnectedToServer?.Invoke();
        server = PhotonNetwork.CloudRegion;
        _uiServerText.text = server;
    }
}
