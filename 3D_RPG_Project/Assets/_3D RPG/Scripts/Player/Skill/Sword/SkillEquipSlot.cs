using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : MonoBehaviour
{
    [SerializeField] string _equipSkillName = null;
    [SerializeField] Image _imgEquipSlot = null;

    [SerializeField] int _skillID = 0;

    bool _isEquipSkill = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_equipSkillName != "")
            _isEquipSkill = true;
    }

    public void PushEquipSlot(int id, string name, Sprite sprite)
    {
        _skillID = id;
        _equipSkillName = name;
        _imgEquipSlot.sprite = sprite;
        _imgEquipSlot.gameObject.SetActive(true);
        _isEquipSkill = true;
    }

    public void RemoveEquipSlot()
    {
        _skillID = -1;
        _equipSkillName = "";
        _isEquipSkill = false;
        _imgEquipSlot.gameObject.SetActive(false);
    }

    public bool IsEquipSkillSlot()
    {
        return _isEquipSkill;
    }

    public string GetSkillName()
    {
        return _equipSkillName;
    }

    public int GetSkillID()
    {
        return _skillID;
    }
}
