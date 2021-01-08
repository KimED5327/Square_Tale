using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageSkillButtonSlot : MonoBehaviour
{
    [SerializeField] string _buttonSkillName = null;
    [SerializeField] Image _imgButtonSlot = null;

    [SerializeField] int _skillID = 0;

    bool _isButtonSkill = false;



    // Start is called before the first frame update
    void Start()
    {
        if (_buttonSkillName != "")
            _isButtonSkill = true;
    }


    public void PushButtonSlot(int id, string name, Sprite sprite)
    {
        _skillID = id;
        _buttonSkillName = name;
        _imgButtonSlot.sprite = sprite;
        _imgButtonSlot.gameObject.SetActive(true);
        _isButtonSkill = true;
    }

    public void RemoveButtonSlot()
    {
        _skillID = -1;
        _buttonSkillName = "";
        _isButtonSkill = false;
        _imgButtonSlot.gameObject.SetActive(false);
    }

    public bool IsButtonSkillSlot()
    {
        return _isButtonSkill;
    }

    public string GetSkillName()
    {
        return _buttonSkillName;
    }

    public int GetSkillID()
    {
        return _skillID;
    }
}
