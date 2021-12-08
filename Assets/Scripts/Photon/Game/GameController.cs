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
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
