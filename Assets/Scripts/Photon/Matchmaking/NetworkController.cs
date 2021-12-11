using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] Text _uiServerText;
    private string server;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.ConnectingToMasterServer)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        server = PhotonNetwork.CloudRegion;
        _uiServerText.text = server;
    }
}
