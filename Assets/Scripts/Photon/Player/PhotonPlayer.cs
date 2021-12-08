using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonPlayer : MonoBehaviour
{
    PhotonView _myPV;
    GameObject _playerAvatar;
    [SerializeField] private GameObject _playerPresence;

    Photon.Realtime.Player[] allPlayers;
    int myNumber;

    private Vector3 _presencePosition;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Waiting Room")
        {
            _presencePosition = _playerPresence.transform.position;
            _myPV = GetComponent<PhotonView>();
            if (_myPV.IsMine)
            {
                _playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), _presencePosition, Quaternion.identity);
                _playerAvatar.transform.localScale += new Vector3(20f, 20f, 0f);
            }
            return;
        }

        allPlayers = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumber++;
            }
        }
         _playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), LevelSpawnPionts.Instance.spawnPoints[myNumber].position, Quaternion.identity);
    }
}
