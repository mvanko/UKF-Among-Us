using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateGameScript : MonoBehaviourPunCallbacks
{
    [SerializeField] int RoomSize; // manu·lne nastavenie hr·Ëa v roomke
    public static int roomNumber;

    public void CreateRoom()
    {
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        roomNumber = randomRoomNumber;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
}
