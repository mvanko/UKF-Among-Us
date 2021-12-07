using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviour
{
    void Start()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        Vector3 pl_position = new Vector3(7.5f, 31.0f, 0.0f);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), pl_position, Quaternion.identity);
    }
}
