using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform roomsContainer;
    [SerializeField] private GameObject roomListingPrefab;
    [SerializeField] private Button _uiJoinMultiplayerButton;
    [SerializeField] private Button _uiCreateRoomButton;
    [SerializeField] private Button _uiChangeNameButton;
    [SerializeField] Text _uiErrorText;

    public InputField _uiPlayerNameInput;
    public InputField _uiRoomNameInput;
    public InputField _uiRoomSizeInput;

    private bool connected = false;
    private bool joined = false;
    private string roomName;
    private int roomSize;

    private List<RoomInfo> roomListing;

    private void Start()
    {
        _uiJoinMultiplayerButton.onClick.AddListener(JoinLobbyOnClick);
        _uiCreateRoomButton.onClick.AddListener(CreateRoom);
        _uiChangeNameButton.onClick.AddListener(PlayerNameUpdate);

        roomListing = new List<RoomInfo>();

        if (PlayerPrefs.HasKey("Nickname"))
        {
            if (PlayerPrefs.GetString("NickName") == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
        }
        _uiPlayerNameInput.text = PhotonNetwork.NickName;
    }

    public override void OnDisable()
    {
        _uiJoinMultiplayerButton.onClick.RemoveListener(JoinLobbyOnClick);
        _uiCreateRoomButton.onClick.RemoveListener(CreateRoom);
        _uiChangeNameButton.onClick.RemoveListener(PlayerNameUpdate);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        connected = true;
    }

    public void PlayerNameUpdate()
    {
        string nameInput = _uiPlayerNameInput.text.ToString();
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    public void JoinLobbyOnClick()
    {
        if(PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer) PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomListing.Clear();

        for(int i = roomsContainer.childCount-1; i >= 0; i--)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            if(room.PlayerCount > 0)
            {
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
            roomListing.Add(room);
        }
    }

    public void CreateRoom()
    {
        if(!connected)
        {
            return;
        }

        roomName = _uiRoomNameInput.text.ToString();

        if (roomName == "")
        {
            roomName = "" + Random.Range(1, 10000);
        }

        if(_uiRoomSizeInput.text.ToString() == "")
        {
            roomSize = 4;
        }else{
            roomSize = int.Parse(_uiRoomSizeInput.text.ToString());
        }

        if (roomSize < 3)
        {
            roomSize = 3;

        }else if (roomSize > 10)
        {
            roomSize = 10;
        }

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize, PlayerTtl = 0 };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _uiErrorText.text = "Failed to create a text, please check your internet connection or try to create a room with different name!";
        _uiErrorText.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        if (!joined)
        {
            joined = true;
            PhotonNetwork.LoadLevel(1);
        }
    }
}
