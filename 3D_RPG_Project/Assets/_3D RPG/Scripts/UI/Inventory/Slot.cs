using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{

    [SerializeField] bool isEquipSlot = false;

    // Item
    [SerializeField] int _count = 0;
    [SerializeField] Item _item = null;
    bool hasItem = false;

    // UI
    [SerializeField] Image _imgItem = null;
    [SerializeField] Text _txtItemCount = null;


    // 슬롯 클리어
    public void ClearSlot()
    {
        _count = 0;
        _item = null;

        ShowUI(false);
    }

    // 아이템 푸시
    public void PushSlot(Item item, int count = 1)
    {
        _item = item;
        _count = count;
        
        ShowUI(true);
    }

    // 슬롯 아이템 개수 증가
    public void IncreaseSlotCount(int count = 1)
    {
        _count += count;

        ShowUI(true);
    }
   

    // 슬롯 아이템 개수 감소
    public void DecreaseCount(int count = 1)
    {
        _count -= count;

        if(_count <= 0)
            ClearSlot();
        else
            ShowUI(true);
    }

    // UI 세팅
    void ShowUI(bool flag)
    {
        hasItem = flag;

        if (hasItem)
        {
            _imgItem.sprite = _item.sprite;
            _imgItem.gameObject.SetActive(true);
            if(_item.type == ItemType.ETC)
            {
                _txtItemCount.gameObject.SetActive(true);
                _txtItemCount.text = _count.ToString();
            }
            else
                _txtItemCount.gameObject.SetActive(false);
        }
        else
        {
            _imgItem.sprite = null;
            _txtItemCount.gameObject.SetActive(false);
        }
    }

    public bool IsSameItem(Item Item) { return _item.id == Item.id; }           // 같은 아이템인지 체크
    public bool IsEmptySlot() { return !hasItem; }                              // 빈 슬롯인지 체크
    public bool HasEnoughCount(int num) { return (_count >= num); }             // num 이상 가지고 있는지 체크
    public int GetSlotCount() { return _count; }                                // 슬롯 아이템 개수 확인
    public Item GetSlotItem() { return _item; }                                 // 슬롯 아이템 리턴


    public void OnPointerClick(PointerEventData eventData)
    {

        if (!Shop._isShow)
        {
            if (hasItem)
                SlotToolTip.instance.ShowToolTip(_item, transform.position, isEquipSlot);
            else
                SlotToolTip.instance.HideToolTip();
        }
        else
        {
            if (hasItem)
                ShopToolTip.instance.ShowToolTip(_item, false);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Shop._isShow)
            SlotToolTip.instance.HideToolTip();
        else
            ShopToolTip.instance.HideToolTip();
    }
}
