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
    int myNumber = 0;

    private Vector3 _presencePosition;

    // Start is called before the first frame update
    void Start()
    {
        _myPV = GetComponent<PhotonView>();
        if (SceneManager.GetActiveScene().name == "Waiting Room")
        {
            _presencePosition = _playerPresence.transform.position;
            if (_myPV.IsMine)
            {
                _playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), _presencePosition, Quaternion.identity);
                _playerAvatar.transform.localScale += new Vector3(20f, 20f, 0f);
                return;
            }
            return;
        }

        allPlayers = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                if (_myPV.IsMine)
                {
                    _playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameManager.Instance.SpawnPoints[myNumber].position, Quaternion.identity);
                    myNumber++;
                }
            }
        }

    }
}
