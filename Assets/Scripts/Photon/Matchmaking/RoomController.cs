using Photon.Pun;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    PhotonView _PV;

    [SerializeField] Text _uiRoomNameText;
    [SerializeField] Text _uiRoomSizeText;
    [SerializeField] Text _uiCountdownText;
    [SerializeField] GameObject _uiGameStartObject;
    [SerializeField] GameObject _uiGameStartTimerObject;
    [SerializeField] Button _uiStartGameButton;
    [SerializeField] Button _uiLeaveGameButton;

    private const int LEVEL1 = 2;
    private const int LEVEL0 = 0;

    private bool startCountdown = false;
    private float timeToStart;

    private void Start()
    {
        _PV = GetComponent<PhotonView>();

        timeToStart = 6;

        CreatePlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            _uiStartGameButton.onClick.AddListener(StartCountdown);
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

    public void StartCountdown()
    { 
        _PV.RPC("Countdown", RpcTarget.All);   
    }

    [PunRPC]
    void Countdown()
    {
        startCountdown = !startCountdown;
        _uiGameStartTimerObject.SetActive(true);
    }

    public void StartGame()
    { 
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(LEVEL1);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        base.OnLeftRoom();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetText();

        if (PhotonNetwork.IsMasterClient)
        {
            _uiStartGameButton.onClick.AddListener(StartCountdown);
            _uiStartGameButton.enabled = true;
        }
    }

    private void Update()
    {
        _uiGameStartObject.SetActive(PhotonNetwork.IsMasterClient);

        if (startCountdown)
        {
            timeToStart -= Time.deltaTime;
            _uiCountdownText.text = ((int)timeToStart).ToString();
        }

        if (timeToStart <= 0)
        {
            _uiCountdownText.enabled = false;
            timeToStart = 999;
            PhotonNetwork.AutomaticallySyncScene = true;
            StartGame();
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
}
