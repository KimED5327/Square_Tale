using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    public Sprite[] _spriteItem;

    private void Awake()
    {
        instance = this;
    }

    public Sprite GetItemSprite(int itemId)
    {
        return _spriteItem[itemId - 2];
    }
}
