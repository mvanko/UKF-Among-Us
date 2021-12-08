using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameData _gameData;

    private List<Player> _activePlayers = new List<Player>();

    public GameData GameData => _gameData;
    public PlayerData PlayerData => _gameData.playerData;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SpawnNewPlayer();
    }

    public void SpawnNewPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
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
