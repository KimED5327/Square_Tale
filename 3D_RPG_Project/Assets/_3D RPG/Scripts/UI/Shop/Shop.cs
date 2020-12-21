using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public int[] item;
}

public class Shop : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] GameObject _goShopPanel = null;

    [Header("Slot")]
    [SerializeField] GameObject _goSlotPrefab = null;
    [SerializeField] Transform _tfSlotParent = null;
    [SerializeField] int _slotMaxCount = 12;
    ShopSlot[] _slots;

    // Json 연동 전 임시
    [Header("Tab")]
    [SerializeField] ShopItem[] _shopItemId = null;
    [SerializeField] Image[] _tabButton = null;
    Color whiteColor = Color.white;
    Color grayColor = Color.gray;

    int _tabNum = 0;
    bool _isShow = false;

    ZoomNPC _npc = null;


    void Awake()
    {
        _slots = new ShopSlot[_slotMaxCount];
        for(int i = 0; i < _slotMaxCount; i++)
        {
            ShopSlot slot = Instantiate(_goSlotPrefab, _tfSlotParent).GetComponent<ShopSlot>();
            _slots[i] = slot;
        }

        TouchTabBtn(0);
    }

    public void CallMenu(ZoomNPC npc = null)
    {
        if(_npc == null)
            _npc = npc;
        _isShow = !_isShow;

        if (_isShow) ShowMenu();
        else HideMenu();
    }

    void ShowMenu()
    {
        GameHudMenu.instance.HideMenu();
        _goShopPanel.SetActive(true);
        TabItemPush();
    }

    public void HideMenu()
    {
        GameHudMenu.instance.ShowMenu();
        _isShow = false;
        _npc.ZoomOutNPC();
        _goShopPanel.SetActive(false);
    }

    public void TouchTabBtn(int num)
    {
        _tabNum = num;

        for(int i = 0; i < _tabButton.Length; i++)
        {
            _tabButton[i].color = grayColor;
        }
        _tabButton[_tabNum].color = whiteColor;

        TabItemPush();
    }

    // 카테고리 별 아이템 분류
    void TabItemPush()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ClearSlot();
            _slots[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < _shopItemId[_tabNum].item.Length; i++)
        {
            _slots[i].gameObject.SetActive(true);
            _slots[i].SetSlot(ItemDatabase.instance.GetItem(_shopItemId[_tabNum].item[i]));
        }
    }
}
