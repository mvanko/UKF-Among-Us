using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] Text _uiRoomNameText;
    [SerializeField] Text _uiRoomSizeText;
    [SerializeField] Button _uiStartGameButton;
    [SerializeField] Button _uiLeaveGameButton;

    private const int LEVEL1 = 2;
    private const int LEVEL0 = 0;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        CreatePlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            _uiStartGameButton.onClick.AddListener(StartGame);
            _uiStartGameButton.enabled = true;
        }
        else
        {
            _uiStartGameButton.enabled = false;
        }

        _uiLeaveGameButton.onClick.AddListener(LeaveGame);
        SetText();
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        SetText();
    }

    public void SetText()
    {
        string roomSize = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        string playerCount = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        string roomName = PhotonNetwork.CurrentRoom.Name;

        _uiRoomNameText.text = roomName;
        _uiRoomSizeText.text = playerCount + "/" + roomSize;
    }

    void CreatePlayer()
    {
       Vector3 _roomPosition = new Vector3(531f, 575f, 0f);
       PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), _roomPosition, Quaternion.identity);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(LEVEL1);
        }
        
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
        //PhotonNetwork.LoadLevel(0);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        base.OnLeftRoom();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _uiStartGameButton.enabled = true;
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
}
