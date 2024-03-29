﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    [SerializeField] BlockSlot[] _slots = null;
    [SerializeField] BlockEquipSlot[] _equipSlots = null;

    static int[] _blockCount = null;

    public static void SaveBlockCount()
    {
        for (int i = 0; i < _blockCount.Length; i++)
        {
            PlayerPrefs.SetInt("block" + i, _blockCount[i]);
        }
    }
    public static void LoadBlockCount()
    {
        if (PlayerPrefs.HasKey("block0"))
        {
            for (int i = 0; i < _blockCount.Length; i++)
                _blockCount[i] = PlayerPrefs.GetInt("block" + i);
        }
        else
            SaveBlockCount();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("blockEquip1", _equipSlots[0].GetBlockID());
        PlayerPrefs.SetInt("blockEquip2", _equipSlots[1].GetBlockID());

        PlayerPrefs.SetString("blockCheck1", _slots[0].GetIsEquip().ToString());
        PlayerPrefs.SetString("blockCheck2", _slots[1].GetIsEquip().ToString());
        PlayerPrefs.SetString("blockCheck3", _slots[2].GetIsEquip().ToString());
        PlayerPrefs.SetString("blockCheck4", _slots[3].GetIsEquip().ToString());
    }

    public void Load()
    {
        EquipBlock(PlayerPrefs.GetInt("blockEquip1"));
        EquipBlock(PlayerPrefs.GetInt("blockEquip2"));

        _slots[0].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("blockCheck1")));
        _slots[1].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("blockCheck2")));
        _slots[2].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("blockCheck3")));
        _slots[3].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("blockCheck4")));
    }

    public static void IncreaseBlockCount(int id, int count)
    {
        _blockCount[id] += count;
        SaveBlockCount();
    }

    public static void AllIncreaseBlockCount(int count)
    {
        for(int i = 0; i < _blockCount.Length; i++)
        {
            _blockCount[i] += count;
        }
        SaveBlockCount();
    }

    public static void DecreaseBlockCount(int id, int count)
    {
        _blockCount[id] -= count;
        SaveBlockCount();
    }

    private void Awake()
    {
        _blockCount = new int[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
        {
            if (!PlayerPrefs.HasKey(StringManager.block + i))
            {
                PlayerPrefs.SetInt(StringManager.block + i, 0);
                _blockCount[i] = 0;
            }
            else
            {
                _blockCount[i] = PlayerPrefs.GetInt(StringManager.block + i);
            }
        }

        LoadBlockCount();
    }

    public void Setting()
    {
        for(int i = 0; i <_slots.Length; i++)
        {
            _slots[i].SettingSlot(_blockCount[i], i);
        }
    }

    public void BtnSlotTouch(int idx)
    {
        bool isEquip = _slots[idx].TryEquip();

        // 블록 장착
        if (isEquip)
            EquipBlock(idx);
        else
            UnEquipBlock(idx);
    }

    void EquipBlock(int idx)
    {
        if (!CanAddEquip())
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotBlockEquip);
            _slots[idx].FailEquip();
            return;
        }

        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (_equipSlots[i].IsEquipBlockSlot()) continue;

            _slots[idx].SetIndex(i);
            string name = _slots[idx].GetBlockName();
            Sprite sprite = _slots[idx].GetBlockSprite();
            _equipSlots[i].PushEquipSlot(idx, name, sprite);
            break;
        }
    }

    void UnEquipBlock(int idx)
    {
        int equipIndex = _slots[idx].GetIndex();
        _equipSlots[equipIndex].RemoveEquipSlot();
    }

    bool CanAddEquip()
    {
        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (!_equipSlots[i].IsEquipBlockSlot()) 
                return true;
        }

        return false;
    }


    /// <summary>
    /// 0 : 1번킷(가로) , 1 : 2번킷(세로)
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public string GetBlockName(int idx)
    {
        int id = _equipSlots[idx].GetBlockID();

        if(id < 0)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgEmptyBlockSlot);
            return "";
        }
        if(_blockCount[id] <= 0)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughBlock);
            return "";
        }
        else
        {
            DecreaseBlockCount(id, 1);
        }

        return _equipSlots[idx].GetBlockName();
    }

    public string GetBlockNameID(int blockID)
    {
        return _slots[blockID].GetBlockName();
    }
}
