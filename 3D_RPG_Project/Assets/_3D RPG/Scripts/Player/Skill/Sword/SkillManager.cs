using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] SkillSlot[] _slots = null;
    [SerializeField] SkillEquipSlot[] _equipSlots = null;
    [SerializeField] SkillButtonSlot[] _buttonSlots = null;

    static int[] _SkillCount = null;

    PlayerMove _player;

    public void Save()
    {
        PlayerPrefs.SetInt("swordSkillEquip1", _equipSlots[0].GetSkillID());
        PlayerPrefs.SetInt("swordSkillEquip2", _equipSlots[1].GetSkillID());

        PlayerPrefs.SetString("swordSkillCheck1", _slots[0].GetIsEquip().ToString());
        PlayerPrefs.SetString("swordSkillCheck2", _slots[1].GetIsEquip().ToString());
        PlayerPrefs.SetString("swordSkillCheck3", _slots[2].GetIsEquip().ToString());
        PlayerPrefs.SetString("swordSkillCheck4", _slots[3].GetIsEquip().ToString());
    }

    public void Load()
    {
        if (PlayerPrefs.GetInt("swordSkillEquip1") != -1)
        {
            EquipSkill(PlayerPrefs.GetInt("swordSkillEquip1"));
        }
        if (PlayerPrefs.GetInt("swordSkillEquip2") != -1)
        {
            EquipSkill(PlayerPrefs.GetInt("swordSkillEquip2"));
        }

        _slots[0].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("swordSkillCheck1")));
        _slots[1].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("swordSkillCheck2")));
        _slots[2].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("swordSkillCheck3")));
        _slots[3].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("swordSkillCheck4")));
    }

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
        _player = FindObjectOfType<PlayerMove>();
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
        //int _index = 0;
        //string name = _slots[_index].GetSkillName();
        //Sprite sprite = _slots[_index].GetSkillSprite();
        //_equipSlots[_index].PushEquipSlot(_index, name, sprite);
        //_buttonSlots[_index].PushButtonSlot(_index, name, sprite);

        //_index = 1;
        //name = _slots[_index].GetSkillName();
        //sprite = _slots[_index].GetSkillSprite();
        //_equipSlots[_index].PushEquipSlot(_index, name, sprite);
        //_buttonSlots[_index].PushButtonSlot(_index, name, sprite);
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
        if (!_player.GetIsSkill1() && !_player.GetIsSkill2() && !_player.GetIsSkill3() && !_player.GetIsSkill4())
        {
            bool isEquip = _slots[idx].TryEquip();

            // 스킬 장착
            if (isEquip)
                EquipSkill(idx);
            else
                UnEquipSkill(idx);
        }
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSwap);
        }
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

    public string GetSkillButtonName(int idx)
    {
        return _buttonSlots[idx].GetSkillName();
    }
}
