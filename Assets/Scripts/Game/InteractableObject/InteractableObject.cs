using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Minigame miniGame;
    [SerializeField] private GameObject highlight;

    private bool isMinigameSpawned = false;
    public bool IsMinigameSpawned => isMinigameSpawned;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            highlight.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    private void MinigameClosed(Player player)
    {
        isMinigameSpawned = false;
        player.MiniGameClosed();
    }

    public void PlayMiniGame(Player player)
    {
        isMinigameSpawned = true;
        Instantiate(miniGame, player.transform.position, Quaternion.identity).Setup(() => { MinigameClosed(player); });
    }
}
