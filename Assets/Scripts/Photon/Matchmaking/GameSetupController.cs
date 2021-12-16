using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetupController : MonoBehaviour
{
    [SerializeField] Button _uiNextButton;

    private const string LEVEL1 = "Level";

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void OnEnable()
    {
        _uiNextButton.onClick.AddListener(NextScene);
    }

    private void OnDisable()
    {
        _uiNextButton.onClick.RemoveListener(NextScene);
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(LEVEL1);
    }
}
