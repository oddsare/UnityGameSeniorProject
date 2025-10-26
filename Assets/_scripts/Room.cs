using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Room : MonoBehaviour
{
    public int index;
    public int value;

    public SpriteRenderer spriteR;

    public void makeSRicons(Sprite icon)
    {
        spriteR.sprite = icon;
    }
}

