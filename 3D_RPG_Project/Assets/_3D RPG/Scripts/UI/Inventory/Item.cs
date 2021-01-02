using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string iconName;
    public bool canSell;
    public string desc;
    public bool stackable;
    public int stack;
    public int levelLimit;
    public ItemType type;
    public ItemCategory category;
    //public Sprite sprite;
    public int priceBuy;
    public int priceSell;
    public List<Option> options;
}

[System.Serializable]
public class Option
{
    public OptionType opType;
    public float num;
}

public enum OptionType
{
    HP,
    STR,
    INT,
    DEF,
    SPEED,
}


public enum ItemCategory
{
    WEAPON,
    ARMOR,
    ETC,
}

public enum ItemType
{
    QUEST = 1,
    WEAPON,
    STAFF,
    ARMOR,
    SUDAN,
    ETC,
}
