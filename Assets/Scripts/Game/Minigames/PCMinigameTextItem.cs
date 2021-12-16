using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PCMinigameTextItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;

    public void Setup(string text)
    {
        uiText.text = text;
    }
}
