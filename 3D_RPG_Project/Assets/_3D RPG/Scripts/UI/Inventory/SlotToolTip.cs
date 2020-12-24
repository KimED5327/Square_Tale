using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public static SlotToolTip instance;

    [Header("UI")]
    [SerializeField] GameObject goToolTip = null;
    [SerializeField] GameObject goEquipButton = null;
    [SerializeField] Text txtName = null;
    [SerializeField] Text txtType = null;
    [SerializeField] Image imgIcon = null;
    [SerializeField] Text txtOption = null;
    [SerializeField] Text txtDesc = null;

    [Header("Button")]
    [SerializeField] Image[] imgBtn = null;
    private readonly int EQUIP = 0, UNEQUIP = 1;
    bool _canEquip = false;
    bool _canUnEquip = false;

    [Header("Offset")]
    [SerializeField] float offsetRightX = 210f;
    [SerializeField] float offsetLeftX = 455f;
    [SerializeField] Vector3 weaponOffset = Vector3.zero;
    [SerializeField] Vector3 armorOffset = Vector3.zero;


    [SerializeField] float offsetY = -150f;
    [SerializeField] float mirrorLinePosX = 0f;

    Item _touchItem;

    Inventory theInven;
    Equipment theEquip; 

    void Awake()
    {
        theInven = GetComponentInParent<Inventory>();
        theEquip = GetComponentInParent<Equipment>();
        instance = this;
    }

    // 툴팁 출력
    public void ShowToolTip(Item item, Vector3 pos, bool isEquipSlot)
    {
        _touchItem = item;

        // 툴팁 내용 세팅
        txtName.text = item.name;
        txtDesc.text = item.desc;
        txtType.text = (item.type == ItemType.WEAPON) ? "무기 류"
                     : (item.type == ItemType.ARMOR) ? "방어구 류"
                     : (item.type == ItemType.ETC) ? "재화 류"
                     : "기타 류";
        imgIcon.sprite = item.sprite;
        txtOption.text = "";
        if (item.options.Count > 0)
        {
            for(int i = 0; i < item.options.Count; i++)
                txtOption.text += item.options[i].name + " " + item.options[i].num + " ";
        }

        // 툴팁 위치 세팅
        if (!isEquipSlot)
        { 
            pos.x += (pos.x >= mirrorLinePosX) ? offsetLeftX : offsetRightX;
            pos.y += offsetY;
        }
        else
            pos = (item.type == ItemType.WEAPON) ? weaponOffset : armorOffset;
        
        goToolTip.transform.position = pos;
        goToolTip.SetActive(true);

        // 장비템이면 장착 해제 버튼 출력
        if (item.type != ItemType.ETC)
        {
            goEquipButton.SetActive(true);

            // 장착 가능
            if (isEquipSlot)
            {
                imgBtn[EQUIP].color = Color.gray;
                imgBtn[UNEQUIP].color = Color.white;
                _canUnEquip = true;
                _canEquip = false;
            }
            // 탈착 가능
            else
            {
                imgBtn[EQUIP].color = Color.white;
                imgBtn[UNEQUIP].color = Color.gray;
                _canUnEquip = false;
                _canEquip = true;
            }
        }
        else
        {
            _canUnEquip = false;
            _canEquip = false;

            goEquipButton.SetActive(false);
        }
    }

    // 툴팁 종료
    public void HideToolTip()
    {
        goToolTip.SetActive(false);
    }


    // 장착
    public void EquipItem()
    {
        if (_canEquip)
        {
            theInven.RemoveItem(_touchItem);

            Item returnEquipItem = theEquip.TryToEquipSlot(_touchItem);
            // 기존 장착된 것이 있다면 교체
            if (returnEquipItem != null)
                theInven.TryToPushInventory(returnEquipItem);

            theInven.ResortItem();
            HideToolTip();
        }

    }

    // 장착 해제
    public void UnEquipItem()
    {
        if (_canUnEquip)
        {
            Item returnEquipItem = theEquip.TakeOffEquipSlot(_touchItem);
            if (returnEquipItem != null)
                theInven.TryToPushInventory(returnEquipItem);

            theInven.ResortItem();
            HideToolTip();
        }

    }
}
