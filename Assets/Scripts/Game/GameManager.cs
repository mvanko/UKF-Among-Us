using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameData _gameData;

    private List<Player> _activePlayers = new List<Player>();
    private List<PlayerData.Color> _freePlayerColors = new List<PlayerData.Color>();

    public GameData GameData => _gameData;
    public PlayerData PlayerData => _gameData.playerData;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private PlayerData.Color? GetFreePlayerColor()
    {
        if(_freePlayerColors.Count > 0)
        {
            return _freePlayerColors[0];
        }

        return null;
    }

    //TODO
    public void SpawnNewPlayer()
    {
    }

    public void AddActivePlayer(Player player)
    {
        _activePlayers.Add(player);
        _freePlayerColors.Remove(player.PlayerColor);
    }

    public void RemoveActivePlayer(Player player)
    {
        _activePlayers.Remove(player);
    }
}
