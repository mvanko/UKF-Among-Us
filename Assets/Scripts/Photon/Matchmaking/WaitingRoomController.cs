using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;

    [SerializeField] private int menuSceneIndex = 0;
    [SerializeField] private int multiplayerSceneIndex = 2;
    [SerializeField] private int minPlayers;
    [SerializeField] private Text roomPlayersCountDisplay;
    [SerializeField] private Text timerToStartDisplay;
    [SerializeField] private Text roomNumber;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float maxFullLobbyWaitTime;
    [SerializeField] private Button _uiLeaveButton;

    private int playerCount;
    private int roomSize;

    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;
    private float notFullLobbyTimer;
    private float fullLobbyTimer;
   
    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullLobbyTimer = maxFullLobbyWaitTime;
        notFullLobbyTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;
        roomNumber.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountUpdate();
    }

    private void OnEnable()
    {
        _uiLeaveButton.onClick.AddListener(LeaveRoom);
    }

    private void OnDisable()
    {
        _uiLeaveButton.onClick.RemoveListener(LeaveRoom);
    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomPlayersCountDisplay.text = playerCount + "/" + roomSize;

        if (playerCount == roomSize)
            readyToStart = true;
        else if (playerCount >= minPlayers)
            readyToCountDown = true;
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullLobbyTimer = timeIn;
        if (timeIn < fullLobbyTimer)
            fullLobbyTimer = timeIn;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    private void WaitingForMorePlayers()
    {
        if (playerCount <= 1)
            ResetTimer();
        if (readyToStart)
        {
            fullLobbyTimer -= Time.deltaTime;
            timerToStartGame = fullLobbyTimer;
        }else if (readyToCountDown)
        {
            notFullLobbyTimer -= Time.deltaTime;
            timerToStartGame = notFullLobbyTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;
        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
            
        }
    }

    private void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullLobbyTimer = maxWaitTime;
        fullLobbyTimer = maxFullLobbyWaitTime;
    }

    private void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
