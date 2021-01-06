﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    public Sprite[] _spriteItem;
    public Sprite[] _spriteBuff;
    public Sprite[] _spriteBlock;


    private void Awake()
    {
        instance = this;
    }

    public Sprite GetItemSprite(int itemId)
    {
        return _spriteItem[itemId - 2];
    }

    public Sprite GetBuffSprite(int buffId)
    {
        return _spriteBuff[buffId - 1];
    }

    public Sprite GetBlockSprite(int blockNum)
    {
        return _spriteBlock[blockNum];
    }
}
