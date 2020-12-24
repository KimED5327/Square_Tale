using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtPrice = null;
    [SerializeField] Image _imgIcon = null;

    Item _item = null;
    bool _isEmptySlot = true;

    public void ClearSlot()
    {
        _item = null;
        _isEmptySlot = true;
        _txtName.text = "-";
        _txtPrice.text = "-";
        _imgIcon.gameObject.SetActive(false);
    }

    public void SetSlot(Item item)
    {
        _isEmptySlot = false;
        _item = item;

        _txtName.text = item.name;
        _txtPrice.text = string.Format("{0:#,##0}", item.price);
        _imgIcon.sprite = item.sprite;
        _imgIcon.gameObject.SetActive(true);
    }

    public int GetItemID() { return _item.id; }
    public bool IsEmpty() { return _isEmptySlot; }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopToolTip.instance.ShowToolTip(_item, true);
    }
}
