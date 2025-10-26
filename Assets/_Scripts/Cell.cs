
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int index;
    public int value;

    public SpriteRenderer Renderer;

    public void SetSpecialRoomSprite(Sprite icon)
    {
        Renderer.sprite = icon;
    }
}
