using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using System.Threading.Tasks;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform[] _levelSpawnPoints;

    private const int NumberOfTasks = 5;
    private int numberOfCompletedTasks = 0;
    private int totalTasks = 0;
    private int impostorCount = 0;
    private List<Minigame> _activeTasks = new List<Minigame>();

    private List<Player> _deadPlayers = new List<Player>();

    public GameData GameData => _gameData;
    public PlayerData PlayerData => _gameData.playerData;
    public MinigameData MinigameData => _gameData.minigameData;
    public Transform[] SpawnPoints => _levelSpawnPoints;
    public int TotalTasks => totalTasks;
    public int TotalTasksCompleted => numberOfCompletedTasks; //TODO SYNC

    public static GameManager Instance { get; private set; }

    private PhotonView _myPV;

    private int impostorNo1, impostorNo2, impostorNo3;

    public static event Action OnGameManagerReady;
    public static event Action OnCanRegisterMinigames;

    public event Action<Minigame> OnMinigameAdded;
    public event Action<Minigame> OnMinigameRemoved;
    public event Action OnTasksUpdated;
    public event Action CrewmatesWon;
    public event Action ImpostersWon;

    private void Awake()
    {
        Instance = this;
        _myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            Player.OnPlayerReady += MakeInitialSyncCalls;
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
            Player.OnPlayerReady -= MakeInitialSyncCalls;
        }
        Player.OnPlayerReady -= RegisterCallbacks;
        if (Player.LocalPlayer != null)
        {
            Player.LocalPlayer.OnMinigameWon -= RemoveActiveTask;
        }
    }

    private void RegisterCallbacks()
    {
        Player.LocalPlayer.OnMinigameWon += RemoveActiveTask;
    }

    public void SpawnNewPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }

    private void MakeInitialSyncCalls()
    {
        int noPlayers = PhotonNetwork.PlayerList.Length;
        int impostorCount = 0;
        if (noPlayers >= 2 && noPlayers <= 4)
        {
            impostorNo1 = UnityEngine.Random.Range(0, noPlayers);
            impostorNo2 = -1;
            impostorNo3 = -1;
            impostorCount = 1;
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
            impostorCount = 2;
        }
        else
        {
            impostorNo1 = UnityEngine.Random.Range(0, noPlayers);
            impostorCount = 1;
        }

        int totalTasks = NumberOfTasks * noPlayers;

        for(int i = 0; i < impostorCount; i++)
        {
            totalTasks -= NumberOfTasks;
        }

        _myPV.RPC("RPC_SyncBully", RpcTarget.All, impostorNo1, impostorNo2, impostorNo3, impostorCount);
        _myPV.RPC("RPC_SyncTotalTasks", RpcTarget.All, totalTasks);
    }

    [PunRPC]
    void RPC_SyncBully(int bullyNo1, int bullyNo2, int bullyNo3, int count)
    {
        Player.LocalPlayer.SetRole(bullyNo1, bullyNo2, bullyNo3);
        impostorCount = count;
    }

    [PunRPC]
    void RPC_SyncTotalTasks(int count)
    {
        totalTasks = count;
        OnTasksUpdated?.Invoke();
    }

    [PunRPC]
    void RPC_SyncTaskCompleted()
    {
        numberOfCompletedTasks++;
        OnTasksUpdated?.Invoke();

        if (PhotonNetwork.IsMasterClient)
        {
            CheckWinForCrewmates();
        }
    }

    [PunRPC]
    void RPC_CrewmatesWon()
    {
        CrewmatesWon?.Invoke();
    }

    [PunRPC]
    void RPC_ImpostorsWon()
    {
        ImpostersWon?.Invoke();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        Player.destroyInstance = true;
        DelayedInstanceDestroy();
    }

    private async void DelayedInstanceDestroy()
    {
        await Task.Delay(500);
        Instance = null;
    }

    private void CheckWinForCrewmates()
    {
        if(totalTasks == numberOfCompletedTasks)
        {
            _myPV.RPC("RPC_CrewmatesWon", RpcTarget.All);
        }
    }

    private void CheckWinForImpostors()
    {
        int aliveCrewmates = PhotonNetwork.CurrentRoom.PlayerCount - _deadPlayers.Count - impostorCount;
        if (aliveCrewmates <= impostorCount)
        {
            _myPV.RPC("RPC_ImpostorsWon", RpcTarget.All);
        }
    }

    public void AddDeadPlayer(Player player)
    {
        _deadPlayers.Add(player);

        if (PhotonNetwork.IsMasterClient)
        {
            CheckWinForImpostors();
        }
    }

    public void AddActiveTask(Minigame minigame)
    {
        _activeTasks.Add(minigame);
        OnMinigameAdded?.Invoke(minigame);
    }

    public void RemoveActiveTask(Minigame minigame)
    {
        _myPV.RPC("RPC_SyncTaskCompleted", RpcTarget.All);
        _activeTasks.Remove(minigame);
        OnMinigameRemoved?.Invoke(minigame);
    }

    public int GetDeadPlayersCount()
    {
        return _deadPlayers.Count;
    }

    public List<int> GetDeadPlayersActorNumbers()
    {
        List<int> deadPlayersActorNumbers = new List<int>();

        foreach(Player p in _deadPlayers)
        {
            int actorNumber = p.GetActorNumber();
            deadPlayersActorNumbers.Add(actorNumber);
        }

        return deadPlayersActorNumbers;
    }
}
