using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour
{
    [Header("Panel UI")]
    [Tooltip("퀘스트 메뉴 Panel")]
    [SerializeField] GameObject _questMenuPanel = null;
    [Tooltip("퀘스트 메뉴 Panel 내 메뉴 프레임 Panel")]
    [SerializeField] GameObject _menuFramePanel = null;

    [Header("ShopSlot")]
    [Tooltip("메뉴 프레임 내 퀘스트 슬롯 Prefab")]
    [SerializeField] GameObject _questSlotPrefab = null;
    [Tooltip("메뉴 프레임 내 슬롯 부모(Content) Transform")]
    [SerializeField] Transform _tfSlotParent = null;
    [SerializeField] int _slotMaxCount = 12;
    ShopSlot[] _slots;

    int _tabNum = 0;
    public static bool _isShow = false;

    void Start()
    {
        //_inven = FindObjectOfType<Inventory>();

        _slots = new ShopSlot[_slotMaxCount];
        for (int i = 0; i < _slotMaxCount; i++)
        {
            ShopSlot slot = Instantiate(_questSlotPrefab, _tfSlotParent).GetComponent<ShopSlot>();
            _slots[i] = slot;
        }

        //TouchTabBtn(0);
    }

    public void CallMenu()
    {
        _isShow = !_isShow;

        if (_isShow) ShowMenu();
        else HideMenu();
    }

    void ShowMenu()
    {
        //BtnBuyWindow();

        GameHudMenu.instance.HideMenu();
        _questMenuPanel.SetActive(true);

        //ReSetUI();
    }

    public void HideMenu()
    {
        GameHudMenu.instance.ShowMenu();

        _isShow = false;

        InteractionManager._isOpen = false;
        _questMenuPanel.SetActive(false);
    }

}
