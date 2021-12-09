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

    private void MinigameClosed()
    {
        isMinigameSpawned = false;
    }

    public void PlayMiniGame(Vector3 playerPosition)
    {
        isMinigameSpawned = true;
        Instantiate(miniGame, playerPosition, Quaternion.identity).Setup(() => { MinigameClosed(); });
    }
}
