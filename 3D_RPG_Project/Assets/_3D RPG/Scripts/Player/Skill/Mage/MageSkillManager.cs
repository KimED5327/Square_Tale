using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageSkillManager : MonoBehaviour
{
    [SerializeField] MageSkillSlot[] _slots = null;
    [SerializeField] MageSkillEquipSlot[] _equipSlots = null;
    [SerializeField] MageSkillButtonSlot[] _buttonSlots = null;

    static int[] _SkillCount = null;


    public static void IncreaseSkillCount(int id, int count)
    {
        _SkillCount[id] += count;
    }

    public static void DecreaseSkillCount(int id, int count)
    {
        _SkillCount[id] -= count;
    }

    private void Awake()
    {
        _SkillCount = new int[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
        {
            if (!PlayerPrefs.HasKey(StringManager.skill + i))
            {
                PlayerPrefs.SetInt(StringManager.skill + i, 0);
                _SkillCount[i] = 0;
            }
            else
            {
                _SkillCount[i] = PlayerPrefs.GetInt(StringManager.skill + i);
            }
        }

        // 임시 등록
        int index = 0;
        string name = _slots[index].GetSkillName();
        Sprite sprite = _slots[index].GetSkillSprite();
        _equipSlots[index].PushEquipSlot(index, name, sprite);
        _buttonSlots[index].PushButtonSlot(index, name, sprite);

        index = 1;
        name = _slots[index].GetSkillName();
        sprite = _slots[index].GetSkillSprite();
        _equipSlots[index].PushEquipSlot(index, name, sprite);
        _buttonSlots[index].PushButtonSlot(index, name, sprite);
    }

    public void Setting()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SettingSlot(_SkillCount[i], i);
        }
    }

    public void BtnSlotTouch(int idx)
    {
        bool isEquip = _slots[idx].TryEquip();

        // 스킬 장착
        if (isEquip)
            EquipSkill(idx);
        else
            UnEquipSkill(idx);
    }

    void EquipSkill(int idx)
    {
        if (!CanAddEquip())
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotBlockEquip);
            _slots[idx].FailEquip();
            return;
        }

        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (_equipSlots[i].IsEquipSkillSlot()) continue;

            _slots[idx].SetIndex(i);
            string name = _slots[idx].GetSkillName();
            Sprite sprite = _slots[idx].GetSkillSprite();
            _equipSlots[i].PushEquipSlot(idx, name, sprite);
            break;
        }

        for (int i = 0; i < _buttonSlots.Length; i++)
        {
            if (_buttonSlots[i].IsButtonSkillSlot()) continue;

            _slots[idx].SetIndex(i);
            string name = _slots[idx].GetSkillName();
            Sprite sprite = _slots[idx].GetSkillSprite();
            _buttonSlots[i].PushButtonSlot(idx, name, sprite);
            break;
        }
    }

    void UnEquipSkill(int idx)
    {
        int equipIndex = _slots[idx].GetIndex();
        _equipSlots[equipIndex].RemoveEquipSlot();
        _buttonSlots[equipIndex].RemoveButtonSlot();
    }

    bool CanAddEquip()
    {
        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (!_equipSlots[i].IsEquipSkillSlot())
                return true;
        }

        for (int i = 0; i < _buttonSlots.Length; i++)
        {
            if (!_buttonSlots[i].IsButtonSkillSlot())
                return true;
        }

        return false;
    }


    /// <summary>
    /// 0 : 1번킷(가로) , 1 : 2번킷(세로)
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public string GetSkillName(int idx)
    {
        int id = _equipSlots[idx].GetSkillID();

        if (id < 0)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgEmptySkillSlot);
            return "";
        }
        if (_SkillCount[id] <= 0)
        {
            //Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughBlock);
            // return "";
        }
        else
        {
            DecreaseSkillCount(id, 1);
        }

        return _equipSlots[idx].GetSkillName();
    }

    public string GetSkillButtonName(int idx)
    {
        int id = _buttonSlots[idx].GetSkillID();

        if (id < 0)
        {
            //Notification.instance.ShowFloatingMessage(StringManager.msgEmptySkillSlot);
            return "";
        }
        if (_SkillCount[id] <= 0)
        {
            //Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughBlock);
            // return "";
        }
        else
        {
            DecreaseSkillCount(id, 1);
        }

        return _buttonSlots[idx].GetSkillName();
    }
}
