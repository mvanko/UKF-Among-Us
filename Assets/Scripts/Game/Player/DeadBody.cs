using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    [SerializeField] SpriteRenderer _bodySprite;

    public void Setup(Color newColor)
    {
        _bodySprite.color = newColor;
    }

    private void OnEnable()
    {
      if(Player.allBodies != null)
      {
        Player.allBodies.Add(transform);
      }
    }

    public void Report()
    {
      Destroy(gameObject);
    }
}
