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
    [SerializeField] GameObject _goShopBuyUI = null;


    [Header("ShopSlot")]
    [SerializeField] GameObject _goSlotPrefab = null;
    [SerializeField] Transform _tfSlotParent = null;
    [SerializeField] int _slotMaxCount = 12;
    ShopSlot[] _slots;

    // Json 연동 전 임시
    [Header("Tab")]
    [SerializeField] ShopItem[] _shopItemId = null;
    [SerializeField] Image[] _tabButton = null;
    
    [Header("UI")]
    [SerializeField] Text _txtGold = null;
    [SerializeField] Image _imgSellBtn = null;
    [SerializeField] Image _imgBuyBtn = null;

    int _tabNum = 0;
    public static bool _isShow = false;
    bool _isBuy = true;

    Inventory _inven;

    void Start()
    {
        _inven = FindObjectOfType<Inventory>();

        _slots = new ShopSlot[_slotMaxCount];
        for(int i = 0; i < _slotMaxCount; i++)
        {
            ShopSlot slot = Instantiate(_goSlotPrefab, _tfSlotParent).GetComponent<ShopSlot>();
            _slots[i] = slot;
        }

        TouchTabBtn(0);
    }

    public void CallMenu()
    {
        _isShow = !_isShow;

        if (_isShow) ShowMenu();
        else HideMenu();
    }

    void ShowMenu()
    {
        BtnBuyWindow();
        GameHudMenu.instance.HideMenu();
        _goShopPanel.SetActive(true);

        ReSetUI();
    }

    public void HideMenu()
    {
        GameHudMenu.instance.ShowMenu();
        
        // 인벤이 열려있다면 같이 닫는다.
        if (!_isBuy)
        {
            _isBuy = true;
            _inven.HideInven(false);
        }
            
        _isShow = false;

        InteractionManager._isOpen = false; 
        _goShopPanel.SetActive(false);
    }

    public void TouchTabBtn(int num)
    {
        SoundManager.instance.PlayEffectSound("Click");

        _tabNum = num;

        for (int i = 0; i < _tabButton.Length; i++)
            _tabButton[i].color = Color.gray;
        
        _tabButton[_tabNum].color = Color.white;

        // 구매 상태면 상품 아이템 재정렬
        if (_isBuy)
            TabItemPush();
        // 판매 상태면 인벤토리 카테고리 우선 정렬
        else
            _inven.SortItem((ItemCategory)_tabNum);

    }

    // 카테고리 별 아이템 분류
    void TabItemPush()
    {
        // 슬롯 초기화
        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ClearSlot();
            _slots[i].gameObject.SetActive(false);
        }

        // 해당 카테고리의 데이터 개수만큼 슬롯 입력
        for(int i = 0; i < _shopItemId[_tabNum].item.Length; i++)
        {
            _slots[i].gameObject.SetActive(true);
            _slots[i].SetSlot(ItemDatabase.instance.GetItem(_shopItemId[_tabNum].item[i]), _inven.GetGold());
        }
    }

    public void BtnSellWindow()
    {
        SoundManager.instance.PlayEffectSound("Click");
        _imgBuyBtn.color = Color.gray;
        _imgSellBtn.color = Color.white;


        _isBuy = false;
        _goShopBuyUI.SetActive(_isBuy);
        _inven.ShowInven(true);

        // 선택되어 있던 카테고리로 인벤을 재정렬 시킴.
        _inven.SortItem((ItemCategory)_tabNum);
    }
    
    public void BtnBuyWindow()
    {
        SoundManager.instance.PlayEffectSound("Click");
        _imgBuyBtn.color = Color.white;
        _imgSellBtn.color = Color.gray;

        _isBuy = true;
        _inven.HideInven(false);
        _goShopBuyUI.SetActive(_isBuy);
    }


    public void ReSetUI()
    {
        _txtGold.text = string.Format("{0:#,##0}", _inven.GetGold());
        TabItemPush();
    }
}
