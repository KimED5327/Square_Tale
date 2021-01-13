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

    PlayerMove _player;

    public void Save()
    {
        PlayerPrefs.SetInt("mageSkillEquip1", _equipSlots[0].GetSkillID());
        PlayerPrefs.SetInt("mageSkillEquip2", _equipSlots[1].GetSkillID());

        PlayerPrefs.SetString("mageSkillCheck1", _slots[0].GetIsEquip().ToString());
        PlayerPrefs.SetString("mageSkillCheck2", _slots[1].GetIsEquip().ToString());
        PlayerPrefs.SetString("mageSkillCheck3", _slots[2].GetIsEquip().ToString());
        PlayerPrefs.SetString("mageSkillCheck4", _slots[3].GetIsEquip().ToString());
    }

    public void Load()
    {
        if (PlayerPrefs.GetInt("mageSkillEquip1") != -1)
        {
            EquipSkill(PlayerPrefs.GetInt("mageSkillEquip1"));
        }
        if (PlayerPrefs.GetInt("mageSkillEquip2") != -1)
        {
            EquipSkill(PlayerPrefs.GetInt("mageSkillEquip2"));
        }

        _slots[0].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("mageSkillCheck1")));
        _slots[1].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("mageSkillCheck2")));
        _slots[2].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("mageSkillCheck3")));
        _slots[3].SetIsEquip(System.Convert.ToBoolean(PlayerPrefs.GetString("mageSkillCheck4")));
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
        //int index = 0;
        //string name = _slots[index].GetSkillName();
        //Sprite sprite = _slots[index].GetSkillSprite();
        //_equipSlots[index].PushEquipSlot(index, name, sprite);
        //_buttonSlots[index].PushButtonSlot(index, name, sprite);

        //index = 1;
        //name = _slots[index].GetSkillName();
        //sprite = _slots[index].GetSkillSprite();
        //_equipSlots[index].PushEquipSlot(index, name, sprite);
        //_buttonSlots[index].PushButtonSlot(index, name, sprite);
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
            _slots[idx].SetIsEquip(true);
            break;
        }

        for (int i = 0; i < _buttonSlots.Length; i++)
        {
            if (_buttonSlots[i].IsButtonSkillSlot()) continue;

            _slots[idx].SetIndex(i);
            string name = _slots[idx].GetSkillName();
            Sprite sprite = _slots[idx].GetSkillSprite();
            _buttonSlots[i].PushButtonSlot(idx, name, sprite);
            _slots[idx].SetIsEquip(true);
            break;
        }
    }

    void UnEquipSkill(int idx)
    {
        int equipIndex = _slots[idx].GetIndex();
        _equipSlots[equipIndex].RemoveEquipSlot();
        _buttonSlots[equipIndex].RemoveButtonSlot();
        _slots[idx].SetIsEquip(false);
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
