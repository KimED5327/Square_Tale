using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EquipSwapItem
{
    public string name;
    public bool[] isEquipItems;
    public Item[] items;
}

public class Equipment : MonoBehaviour
{
    static readonly int WEAPON = 0, ARMOR = 1;

    // 이후 스왑관련 나오면 변경
    [SerializeField] Slot[] _slots = null;
    [SerializeField] EquipSwapItem[] _equipItems = null;

    int _currentSwapNum = -1;

    [SerializeField] Image[] _imgButtons = null;

    void Awake()
    {
        SwapWeaponNum(0);
    }
    // 장착 시도
    public Item TryToEquipSlot(Item item)
    {

        int slotType = (item.category == ItemCategory.WEAPON) ? 0 : 1;

        // 빈슬롯이면 그대로 푸시
        if (_slots[slotType].IsEmptySlot())
        {
            SetEquipSlot(item, slotType, false);
            return null;
        }
        // 아니면 교체
        else
        {
            Item equipItem = _slots[slotType].GetSlotItem();

            _slots[slotType].ClearSlot();
            SetEquipSlot(item, slotType, false);

            return equipItem;
        }
    }

    void SetEquipSlot(Item item, int slotType, bool isClear)
    {
        if (!isClear)
            _slots[slotType].PushSlot(item);
        else
            _slots[slotType].ClearSlot();

        _equipItems[_currentSwapNum].items[slotType] = item;
        _equipItems[_currentSwapNum].isEquipItems[slotType] = !isClear;
    }

    // 장착 슬롯 빼기
    public Item TakeOffEquipSlot(Item item)
    {
        int slotType = (item.category == ItemCategory.WEAPON) ? 0 : 1;
        Item equipItem = _slots[slotType].GetSlotItem();

        SetEquipSlot(null, slotType, true);
        return equipItem;
    }

    public void SwapWeaponNum(int num)
    {
        _imgButtons[num].color = Color.white;
        _imgButtons[(num + 1) % _imgButtons.Length].color = Color.gray;

        // 슬롯 초기화
        if (_currentSwapNum != num)
        {
            _currentSwapNum = num;

            // 슬롯 초기화
            _slots[WEAPON].ClearSlot();
            _slots[ARMOR].ClearSlot();

            // 캐릭터를 스왑했을때, 스왑한 캐릭터가 장착한 아이템이 있다면 슬롯으로 노출.
            if (_equipItems[_currentSwapNum].isEquipItems[WEAPON])
                _slots[WEAPON].PushSlot(_equipItems[_currentSwapNum].items[WEAPON]);
            if (_equipItems[_currentSwapNum].isEquipItems[ARMOR])
                _slots[ARMOR].PushSlot(_equipItems[_currentSwapNum].items[ARMOR]);
        }
    }

    public int GetClassNum() { return _currentSwapNum; }
}
