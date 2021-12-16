using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public enum PlayerColor
    {
        BLUE,
        GREEN,
        ORANGE,
        PINK,
        RED,
        WHITE,
        YELLOW
    }

    [Header("Colors")]
    public Color blue;
    public Color green;
    public Color orange;
    public Color pink;
    public Color red;
    public Color white;
    public Color yellow;

    [Header("Variables")]
    public float movementSpeed;
}
