using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviourPunCallbacks
{
    [SerializeField]private Text nameText;
    [SerializeField]private Text sizeText;

    [SerializeField] private Button _uiJoinButton;

    private string roomName;
    private int roomSize;
    private int playerCount;

    private bool clicked = false;
    public override void OnEnable()
    {
        _uiJoinButton.onClick.AddListener(JoinRoomOnClick);
    }

    public override void OnDisable()
    {
        _uiJoinButton.onClick.RemoveListener(JoinRoomOnClick);
    }

    public void JoinRoomOnClick()
    {
        if (!clicked)
        {
            clicked = true;
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void SetRoom(string nameInput, int sizeInput, int countInput)
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" +sizeInput;
    }
   
}
