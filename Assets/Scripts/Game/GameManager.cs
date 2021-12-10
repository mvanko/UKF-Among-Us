using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform[] _levelSpawnPoints;

    private List<Player> _activePlayers = new List<Player>();

    private Player playerInstance = new Player();

    public GameData GameData => _gameData;
    public PlayerData PlayerData => _gameData.playerData;
    public Transform[] SpawnPoints => _levelSpawnPoints;

    public static GameManager Instance { get; private set; }

    private PhotonView _myPV;

    private int impostorNo1;
    private int impostorNo2;
    private int impostorNo3;

    private void Awake()
    {
        Instance = this;
        SpawnNewPlayer();
    }

    private void Start()
    {
        _myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            PickBully();
        }
    }

    public void SpawnNewPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }

    void PickBully()
    {
        int noPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        if (noPlayers >= 2 && noPlayers <= 4)
        {
            impostorNo1 = Random.Range(0, noPlayers);
            impostorNo2 = -1;
            impostorNo3 = -1;

        }
        else if (noPlayers > 4 && noPlayers <= 8)
        {
            impostorNo1 = Random.Range(0, noPlayers);
            impostorNo2 = Random.Range(0, noPlayers);
            impostorNo3 = -1;
            while (impostorNo2 == impostorNo1)
            {
                impostorNo2 = Random.Range(0, noPlayers);
            }
        }
        else
        {
            impostorNo1 = Random.Range(0, noPlayers);
            impostorNo2 = Random.Range(0, noPlayers);
            impostorNo3 = Random.Range(0, noPlayers);

            while (impostorNo2 == impostorNo1 || impostorNo2 == impostorNo3)
            {
                impostorNo2 = Random.Range(0, noPlayers);
            }

            while (impostorNo3 == impostorNo1 || impostorNo3 == impostorNo2)
            {
                impostorNo3 = Random.Range(0, noPlayers);
            }
        }
        _myPV.RPC("RPC_SyncBully", RpcTarget.All, impostorNo1, impostorNo2, impostorNo3);
        Debug.Log("Sending RPC: " + impostorNo1 +  " " + impostorNo2 +" " + impostorNo3);
    }

    [PunRPC]
    void RPC_SyncBully(int bullyNo1, int bullyNo2, int bullyNo3)
    {
            playerInstance.SetRole(bullyNo1, bullyNo2, bullyNo3);
    }

    public void AddActivePlayer(Player player)
    {
        _activePlayers.Add(player);
    }

    public void RemoveActivePlayer(Player player)
    {
        _activePlayers.Remove(player);
    }
}
