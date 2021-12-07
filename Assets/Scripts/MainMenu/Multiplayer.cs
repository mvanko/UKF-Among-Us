using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _uiBackButton;

    private string roomName;

    private void OnEnable()
    {
        _uiBackButton.onClick.AddListener(BackToMenu);
    }

    private void OnDisable()
    {
        _uiBackButton.onClick.RemoveListener(BackToMenu);
    }

    private void BackToMenu()
    {
        PhotonNetwork.LeaveLobby();
        this.gameObject.SetActive(false);
    }
}
