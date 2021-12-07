using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject miniGame;
    [SerializeField] GameObject highlight;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            highlight.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    public void PlayMiniGame()
    {
        miniGame.SetActive(true);
    }
}
