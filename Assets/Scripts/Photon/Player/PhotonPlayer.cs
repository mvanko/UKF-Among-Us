using Photon.Pun;
using UnityEngine;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    PhotonView _myPV;
    GameObject _playerAvatar;
    [SerializeField] private GameObject _playerPresence;

    private Vector3 _presencePosition;

    // Start is called before the first frame update
    void Start()
    {
        _presencePosition = _playerPresence.transform.position;
        _myPV = GetComponent<PhotonView>();
        if (_myPV.IsMine)
        {
            _playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), _presencePosition, Quaternion.identity);
            _playerAvatar.transform.localScale += new Vector3(20f,20f,0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
