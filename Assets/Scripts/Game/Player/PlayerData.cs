using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public enum Color
    {
        BLUE,
        GREEN,
        ORANGE,
        PINK,
        RED,
        WHITE,
        YELLOW
    }

    [Header("Alive")]
    public Sprite blueSprite;
    public Sprite greenSprite;
    public Sprite orangeSprite;
    public Sprite pinkSprite;
    public Sprite redSprite;
    public Sprite whiteSprite;
    public Sprite yellowSprite;

    [Header("Dead")]
    public Sprite deadBlueSprite;
    public Sprite deadGreenSprite;
    public Sprite deadOrangeSprite;
    public Sprite deadPinkSprite;
    public Sprite deadRedSprite;
    public Sprite deadWhiteSprite;
    public Sprite deadYellowSprite;

    public Sprite deadBlueLayingSprite;
    public Sprite deadGreenLayingSprite;
    public Sprite deadOrangeLayingSprite;
    public Sprite deadPinkLayingSprite;
    public Sprite deadRedLayingSprite;
    public Sprite deadWhiteLayingSprite;
    public Sprite deadYellowLayingSprite;

    [Header("Variables")]
    public float movementSpeed;
}
