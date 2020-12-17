using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    //private readonly int WEAPON = 0, ARMOR = 1; 

    // 이후 스왑관련 나오면 변경
    [SerializeField] Slot[] slots = null;
    
    // 장착 시도
    public Item TryToEquipSlot(Item item)
    {
        int weaponSlotNum = (item.type == ItemType.WEAPON) ? 0 : 1;

        // 빈슬롯이면 그대로 푸시
        if (slots[weaponSlotNum].IsEmptySlot())
        {
            slots[weaponSlotNum].PushSlot(item);
            return null;
        }
        // 아니면 교체
        else
        {
            Item equipItem = slots[weaponSlotNum].GetSlotItem();
            slots[weaponSlotNum].ClearSlot();

            slots[weaponSlotNum].PushSlot(item);
            return equipItem;
        }
    }

    // 장착 슬롯 빼기
    public Item TakeOffEquipSlot(Item item)
    {
        int weaponSlotNum = (item.type == ItemType.WEAPON) ? 0 : 1;
        Item equipItem = slots[weaponSlotNum].GetSlotItem();

        slots[weaponSlotNum].ClearSlot();
        return equipItem;
    }

}
