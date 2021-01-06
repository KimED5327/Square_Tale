using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    [SerializeField] BlockSlot[] _slots = null;
    [SerializeField] BlockEquipSlot[] _equipSlots = null;

    static int[] _blockCount = null;


    public static void IncreaseBlockCount(int id, int count)
    {
        _blockCount[id] += count;
    }

    public static void DecreaseBlockCount(int id, int count)
    {
        _blockCount[id] -= count;
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

        // 임시 등록
        int index = 0;
        string name = _slots[index].GetBlockName();
        Sprite sprite = _slots[index].GetBlockSprite();
        _equipSlots[index].PushEquipSlot(index, name, sprite);

        index = 1;
        name = _slots[index].GetBlockName();
        sprite = _slots[index].GetBlockSprite();
        _equipSlots[index].PushEquipSlot(index, name, sprite);
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
            // return "";
        }
        else
        {
            DecreaseBlockCount(id, 1);
        }

        return _equipSlots[idx].GetBlockName();
    }
}
