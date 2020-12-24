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
    [SerializeField] Slot[] slots = null;
    [SerializeField] EquipSwapItem[] equipItems = null;

    int _currentSwapNum = -1;

    [SerializeField] Image[] imgButtons = null;

    void Awake()
    {
        SwapWeaponNum(0);
    }
    // 장착 시도
    public Item TryToEquipSlot(Item item)
    {
        int slotType = (item.type == ItemType.WEAPON) ? 0 : 1;

        // 빈슬롯이면 그대로 푸시
        if (slots[slotType].IsEmptySlot())
        {
            SetEquipSlot(item, slotType, false);
            return null;
        }
        // 아니면 교체
        else
        {
            Item equipItem = slots[slotType].GetSlotItem();

            slots[slotType].ClearSlot();
            SetEquipSlot(item, slotType, false);

            return equipItem;
        }
    }

    void SetEquipSlot(Item item, int slotType, bool isClear)
    {
        if (!isClear)
            slots[slotType].PushSlot(item);
        else
            slots[slotType].ClearSlot();

        equipItems[_currentSwapNum].items[slotType] = item;
        equipItems[_currentSwapNum].isEquipItems[slotType] = !isClear;
    }

    // 장착 슬롯 빼기
    public Item TakeOffEquipSlot(Item item)
    {
        int slotType = (item.type == ItemType.WEAPON) ? 0 : 1;
        Item equipItem = slots[slotType].GetSlotItem();

        SetEquipSlot(null, slotType, true);
        return equipItem;
    }

    public void SwapWeaponNum(int num)
    {
        imgButtons[num].color = Color.white;
        imgButtons[(num + 1) % imgButtons.Length].color = Color.gray;

        if (_currentSwapNum != num)
        {
            _currentSwapNum = num;

            // 슬롯 초기화
            slots[WEAPON].ClearSlot();
            slots[ARMOR].ClearSlot();

            // 캐릭터를 스왑했을때, 스왑한 캐릭터가 장착한 아이템이 있다면 슬롯으로 노출.
            if (equipItems[_currentSwapNum].isEquipItems[WEAPON])
                slots[WEAPON].PushSlot(equipItems[_currentSwapNum].items[WEAPON]);
            if (equipItems[_currentSwapNum].isEquipItems[ARMOR])
                slots[ARMOR].PushSlot(equipItems[_currentSwapNum].items[ARMOR]);
        }
    }
}
