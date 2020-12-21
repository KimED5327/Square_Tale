using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string desc;
    public int levelLimit;
    public ItemType type;
    public Sprite sprite;
    public int price;
    public List<Option> options;
}

[System.Serializable]
public class Option
{
    public string name;
    public float num;
}

public enum ItemType
{
    WEAPON,
    ARMOR,
    ETC,
}
