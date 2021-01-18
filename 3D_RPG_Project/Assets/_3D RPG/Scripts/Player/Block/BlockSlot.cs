using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlockSlot : MonoBehaviour
{
    [SerializeField] Image _imgBtn = null;
    [SerializeField] Text _txtButton = null;
    [SerializeField] Text _txtCount = null;
    [SerializeField] Image _imgIcon = null;
    [SerializeField] string _blockName = "";

    [SerializeField] int _equipSlotIndex = -1;

    static readonly string _strEnroll = "블록 등록";
    static readonly string _strCancel = "블록 해제";

    [SerializeField] bool _isEquip = false;

    private void Start()
    {
        _txtButton.text = _isEquip ? _strCancel : _strEnroll;
    }

    public void SettingSlot(int count, int slotIndex)
    {
        _txtCount.text = string.Format("{0:#,##0}", count);
        _imgIcon.sprite = SpriteManager.instance.GetBlockSprite(slotIndex);
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
    public string GetBlockName() { return _blockName; }

    public Sprite GetBlockSprite() { return _imgIcon.sprite; }
    public bool GetIsEquip() { return _isEquip; }
    public void SetIsEquip(bool isEquip) { _isEquip = isEquip; }
}
