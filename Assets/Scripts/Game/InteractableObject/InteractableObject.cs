using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject miniGame;
    [SerializeField] GameObject highlight;

    private bool isHighlightActive = false;
    private bool isMinigameSpawned = false;

    public bool IsHighlightActive => isHighlightActive;
    public bool IsMinigameSpawned => isMinigameSpawned;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            highlight.SetActive(true);
            isHighlightActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
            isHighlightActive = false;
        }
    }

    public void PlayMiniGame(Vector3 playerPosition)
    {
        isMinigameSpawned = true;
        Instantiate(miniGame, playerPosition, Quaternion.identity);
        //miniGame.SetActive(true);
    }
}
