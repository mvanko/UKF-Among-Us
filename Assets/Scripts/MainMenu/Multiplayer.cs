using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour
{
    [SerializeField] private Button _uiBackButton;

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
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby) PhotonNetwork.LeaveLobby();
        this.gameObject.SetActive(false);
    }
}
