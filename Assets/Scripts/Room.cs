using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Room : MonoBehaviour
{
    public int index;
    public int value;

    public SpriteRenderer spriteR;
    public SpriteRenderer roomRenderer;

    public void makeSRicons(Sprite icon)
    {
        spriteR.sprite = icon;
    }

    public void makeRicons(Sprite roomIcon)
    {
        roomRenderer.sprite = roomIcon;
    }


    public void RotateRoom(List<int> connectedRooms)
    {
        connectedRooms.Sort();
        index = connectedRooms[0];

        if(connectedRooms.Contains(index+1)&& connectedRooms.Contains(index + 10))
        {
            makeRotation(-90);
        }
        else if (connectedRooms.Contains(index + 1) && connectedRooms.Contains(index + 11))
        {
            makeRotation(180);
        }
        else if (connectedRooms.Contains(index + 9) && connectedRooms.Contains(index + 10))
        {
            makeRotation(90);
        }
    }

    public void makeRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


}

