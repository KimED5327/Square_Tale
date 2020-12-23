using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinMaxCount
{
    public int min;
    public int max;
}


[System.Serializable]
public class DropItemInfo
{
    public int itemID;
    public MinMaxCount itemCounts;
    public float itemDropRate;
}

[System.Serializable]
public class DropItem
{
    public int itemID;
    public int itemCount;
}