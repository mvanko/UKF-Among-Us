using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] Color[] allColors;

    public void SetColor(int colorIndex)
    {
        Player.LocalPlayer.SetColor(allColors[colorIndex]);
    }
}
