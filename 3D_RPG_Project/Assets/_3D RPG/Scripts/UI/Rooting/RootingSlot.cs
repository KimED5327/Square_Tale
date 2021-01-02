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

    public void SetSlot(Item item, int count, bool isGold)
    {
        _item = item;
        _count = count;

        _txtName.text = _item.name;
        _txtType.text = (_item.type == ItemType.WEAPON) ? "무기"
                      : (_item.type == ItemType.ARMOR)  ? "방어구"
                      : (_item.type == ItemType.ETC)    ? "재화"
                                                        : "기타";

        if(!isGold)
            _txtCount.text = $"X {0}" + _count;
        else
            _txtCount.text = $"{0} Gold" + _count;

        //_ImgIcon.sprite = _item.sprite;
        _ImgIcon.sprite = SpriteManager.instance.GetItemSprite(_item.id);

    }
}
