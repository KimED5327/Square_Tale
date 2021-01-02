using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    static int SLOT_MAX_COUNT = 99;

    [SerializeField] bool _isEquipSlot = false;

    // Item
    [SerializeField] int _count = 0;
    [SerializeField] Item _item = null;
    bool _hasItem = false;

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

    /// <summary>
    /// 슬롯 아이템 증가.
    /// </summary>
    /// <param name="count"></param>
    /// <returns>최대 개수 초과시 초과된 개수</returns>
    public int IncreaseSlotCount(int count = 1)
    {
        _count += count;

        int overCount = 0;

        if(_count > SLOT_MAX_COUNT)
            overCount = _count - SLOT_MAX_COUNT;
       
        ShowUI(true);

        return overCount;
    }
   

    // 슬롯 아이템 개수 감소
    public int DecreaseCount(int count = 1)
    {
        int overCount = 0;

        _count -= count;

        if (_count < 0)
            overCount = Mathf.Abs(_count);

        if(_count <= 0)
            ClearSlot();
        else
            ShowUI(true);

        return overCount;
    }

    // UI 세팅
    void ShowUI(bool flag)
    {
        _hasItem = flag;
        _imgItem.gameObject.SetActive(_hasItem);
        if (_hasItem)
        {
            _imgItem.sprite = _item.sprite;
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
    public bool IsEmptySlot() { return !_hasItem; }                              // 빈 슬롯인지 체크
    public bool HasEnoughCount(int num) { return (_count >= num); }             // num 이상 가지고 있는지 체크
    public int GetSlotCount() { return _count; }                                // 슬롯 아이템 개수 확인
    public Item GetSlotItem() { return _item; }                                 // 슬롯 아이템 리턴


    public void OnPointerClick(PointerEventData eventData)
    {

        if (!Shop._isShow)
        {
            if (_hasItem)
                SlotToolTip.instance.ShowToolTip(_item, transform.position, _isEquipSlot);
            else
                SlotToolTip.instance.HideToolTip();
        }
        else
        {
            if (_hasItem)
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
