using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopSlot : MonoBehaviour
{

    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtPrice = null;
    [SerializeField] Image _imgIcon = null;

    bool isEmptySlot = true;

    public void ClearSlot()
    {
        isEmptySlot = true;
        _txtName.text = "-";
        _txtPrice.text = "-";
        _imgIcon.gameObject.SetActive(false);
    }

    public void SetSlot(Item p_item)
    {
        isEmptySlot = false;

        _txtName.text = p_item.name;
        _txtPrice.text = string.Format("{0:#,##0}", p_item.price);
        _imgIcon.sprite = p_item.sprite;
        _imgIcon.gameObject.SetActive(true);
    }


    public bool IsEmpty() { return isEmptySlot; }

}
