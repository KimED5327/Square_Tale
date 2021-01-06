using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockEquipSlot : MonoBehaviour
{
    [SerializeField] string _equipBlockName = null;
    [SerializeField] Image _imgEquipSlot = null;

    [SerializeField] int _blockID = 0;

    bool _isEquipBlock = false;



    // Start is called before the first frame update
    void Start()
    {
        if (_equipBlockName != "")
            _isEquipBlock = true;
    }


    public void PushEquipSlot(int id, string name, Sprite sprite)
    {
        _blockID = id;
        _equipBlockName = name;
        _imgEquipSlot.sprite = sprite;
        _imgEquipSlot.gameObject.SetActive(true);
        _isEquipBlock = true;
    }

    public void RemoveEquipSlot()
    {
        _blockID = -1;
        _equipBlockName = "";
        _isEquipBlock = false;
        _imgEquipSlot.gameObject.SetActive(false);
    }

    public bool IsEquipBlockSlot()
    {
        return _isEquipBlock;
    }

    public string GetBlockName()
    {
        return _equipBlockName;
    }

    public int GetBlockID()
    {
        return _blockID;
    }
}
