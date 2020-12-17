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
    [SerializeField] Text txtLimitLevel = null;
    [SerializeField] Text txtOption = null;
    [SerializeField] Text txtDesc = null;

    [Header("Button")]
    [SerializeField] Image[] imgBtn = null;
    private readonly int EQUIP = 0, UNEQUIP = 1;


    [Header("Offset")]
    [SerializeField] float offsetRightX = 210f;
    [SerializeField] float offsetLeftX = 455f;

    [SerializeField] float offsetY = -150f;
    [SerializeField] float mirrorLinePosX = 0f;

    Item _touchItem;

    Inventory theInven;
    Equipment theEquip; 

    void Awake()
    {
        theInven = GetComponent<Inventory>();
        theEquip = GetComponent<Equipment>();
        instance = this;
    }

    // 툴팁 출력
    public void ShowToolTip(Item item, Vector3 pos, bool isEquipSlot)
    {
        _touchItem = item;

        // 툴팁 내용 세팅
        txtName.text = item.name;
        txtDesc.text = item.desc;
        txtType.text = "장비류 / 검 류"; // 임시 테스트용
        txtLimitLevel.text = item.levelLimit + "Lv";
        txtOption.text = "";
        if (item.options.Count > 0)
        {
            for(int i = 0; i < item.options.Count; i++)
                txtOption.text += item.options[i].name + " " + item.options[i].num + " ";
        }

        // 툴팁 위치 세팅
        pos.x += (pos.x >= mirrorLinePosX) ? offsetLeftX : offsetRightX;
        pos.y += offsetY;
        goToolTip.transform.localPosition = pos;
        goToolTip.SetActive(true);

        // 장비템이면 장착 해제 버튼 출력
        if (item.type != ItemType.ETC)
        {
            goEquipButton.SetActive(true);

            if (isEquipSlot)
            {
                imgBtn[EQUIP].color = Color.gray;
                imgBtn[UNEQUIP].color = Color.white;
            }
            else
            {
                imgBtn[EQUIP].color = Color.white;
                imgBtn[UNEQUIP].color = Color.gray;
            }
        }
        else
            goEquipButton.SetActive(false);
    }

    // 툴팁 종료
    public void HideToolTip()
    {
        goToolTip.SetActive(false);
    }


    // 장착
    public void EquipItem()
    {
        theInven.RemoveItem(_touchItem);

        Item returnEquipItem = theEquip.TryToEquipSlot(_touchItem);
        // 기존 장착된 것이 있다면 교체
        if (returnEquipItem != null)
            theInven.TryToPushInventory(returnEquipItem);

        theInven.SerializeItem();
        HideToolTip();
    }

    // 장착 해제
    public void UnEquipItem()
    {
        Item returnEquipItem = theEquip.TakeOffEquipSlot(_touchItem);
        if (returnEquipItem != null)
            theInven.TryToPushInventory(returnEquipItem);

        theInven.ResortItem();
        HideToolTip();
    }
}
