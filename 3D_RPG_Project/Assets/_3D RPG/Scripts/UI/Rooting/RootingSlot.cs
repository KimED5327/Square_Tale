using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RootingSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtType = null;
    [SerializeField] Text _txtCount = null;
    [SerializeField] Image _ImgIcon = null;

    Item _item;
    int _count;

    Rooting _rootingUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        _rootingUI.PushInventory(_item, _count);
    }

    public void SetLink(Rooting rooting) => _rootingUI = rooting;

    public void SetSlot(Item item, int count)
    {
        _item = item;
        _count = count;

        if(_item.id == 1)
        {
            _txtName.text = _item.name;
            _txtType.text = "재화";
            _txtCount.text = string.Format("{0:###,0}", _count) + " 골드";
        }
        else
        {
            _txtName.text = _item.name;
            _txtType.text = (_item.type == ItemType.WEAPON) ? "무기"
                          : (_item.type == ItemType.ARMOR) ? "방어구"
                          : (_item.type == ItemType.ETC) ? "재화"
                                                            : "기타";
            _txtCount.text = $"X {0}" + _count;
        }


        //_ImgIcon.sprite = _item.sprite;
        _ImgIcon.sprite = SpriteManager.instance.GetItemSprite(_item.id);

    }
}
