using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    public Sprite[] _spriteItem;
    public Sprite[] _spriteBuff;
    public Sprite[] _spriteBlock;
    public Sprite[] _spriteSwordSkill;
    public Sprite[] _spriteMageSkill;


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

    public Sprite GetSwordSkillSprite(int skillNum)
    {
        return _spriteSwordSkill[skillNum];
    }

    public Sprite GetMageSkillSprite(int skillNum)
    {
        return _spriteMageSkill[skillNum];
    }
}
