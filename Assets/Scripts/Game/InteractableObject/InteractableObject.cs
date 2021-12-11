using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Minigame miniGame;
    [SerializeField] private GameObject highlight;

    [SerializeField] private SpriteRenderer interactableObjectRenderer = null;
    [SerializeField] private Sprite spriteAfterWon = null;

    private bool isMinigameCompleted = false;
    private bool isMinigameSpawned = false;
    private bool isHighlighted = false;
    public bool IsMinigameSpawned => isMinigameSpawned;
    public bool IsMinigameCompleted => isMinigameCompleted;
    public bool IsHighlighted => isHighlighted;

    public static event Action<InteractableObject> OnHighlighted;

    private void Awake()
    {
        GameManager.OnCanRegisterMinigames += RegisterMinigame;
    }

    private void OnDestroy()
    {
        GameManager.OnCanRegisterMinigames -= RegisterMinigame;
    }

    private void RegisterMinigame()
    {
        GameManager.Instance.AddActiveTask(miniGame);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isMinigameCompleted && other.GetComponent<Player>() == Player.LocalPlayer && !Player.LocalPlayer.IsImposter)
        {
            highlight.SetActive(true);
            isHighlighted = true;
            OnHighlighted?.Invoke(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !isMinigameCompleted && other.GetComponent<Player>() == Player.LocalPlayer && !Player.LocalPlayer.IsImposter)
        {
            highlight.SetActive(false);
            isHighlighted = false;
            OnHighlighted?.Invoke(null);
        }
    }

    private void MinigameClosed(Player player)
    {
        isMinigameSpawned = false;
        player.MiniGameClosed();
    }

    private void MinigameWon(Player player)
    {
        isMinigameSpawned = false;
        isMinigameCompleted = true;
        isHighlighted = false;
        highlight.SetActive(false);

        if(spriteAfterWon != null)
        {
            interactableObjectRenderer.sprite = spriteAfterWon;
        }

        player.MiniGameWon(miniGame);
    }

    public void PlayMiniGame(Player player)
    {
        isMinigameSpawned = true;
        Instantiate(miniGame, player.transform.position, Quaternion.identity).Setup(() => { MinigameClosed(player); }, () => { MinigameWon(player); });
    }
}
