using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform[] _levelSpawnPoints;

    [SerializeField] private Canvas _reportCanvas;
    [SerializeField] private Canvas _gameCanvas;

    private const int NumberOfTasks = 4;
    private int numberOfCompletedTasks = 0;
    private List<Minigame> _activeTasks = new List<Minigame>();

    public GameData GameData => _gameData;
    public PlayerData PlayerData => _gameData.playerData;
    public MinigameData MinigameData => _gameData.minigameData;
    public Transform[] SpawnPoints => _levelSpawnPoints;
    public int TotalTasks => NumberOfTasks * PhotonNetwork.PlayerList.Length; //TODO SYNC
    public int TotalTasksCompleted => numberOfCompletedTasks; //TODO SYNC

    public static GameManager Instance { get; private set; }

    private PhotonView _myPV;

    private int impostorNo1, impostorNo2, impostorNo3;

    public static event Action OnGameManagerReady;
    public static event Action OnCanRegisterMinigames;

    public event Action<Minigame> OnMinigameAdded;
    public event Action<Minigame> OnMinigameRemoved;

    private void Awake()
    {
        Instance = this;
        _myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            Player.OnPlayerReady += PickBully;
        }
        Player.OnPlayerReady += RegisterCallbacks;

        SpawnNewPlayer();
    }

    private void Start()
    {
        OnGameManagerReady?.Invoke();
        OnCanRegisterMinigames?.Invoke();
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player.OnPlayerReady -= PickBully;
        }
        Player.OnPlayerReady -= RegisterCallbacks;
        Player.LocalPlayer.OnMinigameWon -= RemoveActiveTask;
    }

    private void RegisterCallbacks()
    {
        Player.LocalPlayer.OnMinigameWon += RemoveActiveTask;
    }

    public void SpawnNewPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }

    private void PickBully()
    {
        int noPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        if (noPlayers >= 2 && noPlayers <= 4)
        {
            impostorNo1 = UnityEngine.Random.Range(0, noPlayers);
            impostorNo2 = -1;
            impostorNo3 = -1;

        }
        else if (noPlayers > 4 && noPlayers <= 8)
        {
            impostorNo1 = UnityEngine.Random.Range(0, noPlayers);
            impostorNo2 = UnityEngine.Random.Range(0, noPlayers);
            impostorNo3 = -1;
            while (impostorNo2 == impostorNo1)
            {
                impostorNo2 = UnityEngine.Random.Range(0, noPlayers);
            }
        }
        else
        {
            impostorNo1 = UnityEngine.Random.Range(0, noPlayers);
        }
        _myPV.RPC("RPC_SyncBully", RpcTarget.All, impostorNo1, impostorNo2, impostorNo3);
    }

    [PunRPC]
    void RPC_SyncBully(int bullyNo1, int bullyNo2, int bullyNo3)
    {
        Player.LocalPlayer.SetRole(bullyNo1, bullyNo2, bullyNo3);
    }

    public void AddActiveTask(Minigame minigame)
    {
        _activeTasks.Add(minigame);
        OnMinigameAdded?.Invoke(minigame);
    }

    public void RemoveActiveTask(Minigame minigame)
    {
        numberOfCompletedTasks++;
        _activeTasks.Remove(minigame);
        OnMinigameRemoved?.Invoke(minigame);
    }
}
