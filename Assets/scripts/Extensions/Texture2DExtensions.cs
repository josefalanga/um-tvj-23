using System;
using UnityEngine;

public static class Texture2DExtensions
{
    public static Sprite GetSprite(this Texture texture)
    {
        return Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
