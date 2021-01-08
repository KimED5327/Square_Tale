using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] Image _imgBtn = null;
    [SerializeField] Text _txtButton = null;
    [SerializeField] Image _imgIcon = null;
    [SerializeField] string _skillName = "";

    [SerializeField] int _equipSlotIndex = -1;

    static readonly string _strEnroll = "스킬 등록";
    static readonly string _strCancel = "스킬 해제";

    [SerializeField] bool _isEquip = false;

    private void Start()
    {
        _txtButton.text = _isEquip ? _strCancel : _strEnroll;
    }

    public void SettingSlot(int count, int slotIndex)
    {
        _imgIcon.sprite = SpriteManager.instance.GetSwordSkillSprite(slotIndex);
        ReUI();
    }



    public bool TryEquip()
    {
        _isEquip = !_isEquip;
        ReUI();
        return _isEquip;
    }

    public void FailEquip()
    {
        _isEquip = !_isEquip;
        ReUI();
    }

    void ReUI()
    {
        _txtButton.text = _isEquip ? _strCancel : _strEnroll;
        _imgBtn.color = _isEquip ? Color.gray : Color.white;
    }

    public void SetIndex(int idx) { _equipSlotIndex = idx; }
    public int GetIndex() { return _equipSlotIndex; }
    public string GetSkillName() { return _skillName; }

    public Sprite GetSkillSprite() { return _imgIcon.sprite; }
}
