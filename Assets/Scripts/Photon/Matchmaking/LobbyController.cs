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

    public InputField _uiPlayerNameInput;
    public InputField _uiRoomNameInput;
    public InputField _uiRoomSizeInput;

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
        int tempIndex;

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room);
            if (roomListing != null)
            {
                tempIndex = roomListing.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }

            if (tempIndex!= -1)
            {
                roomListing.RemoveAt(tempIndex);
                Destroy(roomsContainer.GetChild(tempIndex).gameObject);
            }

            if(room.PlayerCount > 0)
            {
                roomListing.Add(room);
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
        }
    }

    public void CreateRoom()
    {
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

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room!");
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
