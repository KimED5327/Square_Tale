﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopToolTip : MonoBehaviour
{
    public static ShopToolTip instance;

    [Header("UI")]
    [SerializeField] GameObject _goToolTip = null;
    [SerializeField] Text _txtTitle = null;
    [SerializeField] Text _txtName = null;
    [SerializeField] Image _imgIcon = null;
    [SerializeField] Text _txtOption = null;
    [SerializeField] Text _txtDesc = null;
    [SerializeField] Text _txtPrice = null;

    [Header("Button")]
    [SerializeField] Text _txtButton = null;


    [Header("Requestion")]
    [SerializeField] GameObject _goRequestionUI = null;
    [SerializeField] Text _txtRequestion = null;
    string _buyMessage = "해당 아이템을 구매하시겠습니까?";
    string _sellMessage = "해당 아이템을 판매하시겠습니까?";

    Item _touchItem;
    int _price;
    bool _isBuy = false;

    //Component
    Inventory _inven;
    Shop _shop;

    void Awake() {
        _inven = FindObjectOfType<Inventory>();
        _shop = FindObjectOfType<Shop>();
        instance = this; 
    }

    // 툴팁 출력
    public void ShowToolTip(Item item, bool isBuy = true)
    {
        _isBuy = isBuy;
        _touchItem = item;

        // 툴팁 내용 세팅
        _txtName.text = item.name;
        _txtDesc.text = item.desc;
        _price = item.price;

        _txtPrice.text = string.Format("{0:#,##0}", _price);
        _txtPrice.color = (_price > _inven.GetGold()) ? Color.red : Color.yellow;
        
        _txtOption.text = "";
        if (item.options.Count > 0)
        {
            for (int i = 0; i < item.options.Count; i++)
                _txtOption.text += item.options[i].name + " " + item.options[i].num + " ";
        }

        _txtTitle.text = (_isBuy) ? "상품 구매" : "상품 판매";
        _txtButton.text = (_isBuy) ? "구매" : "판매";
        _txtRequestion.text = (_isBuy) ? _buyMessage
                                      : _sellMessage;

        _goToolTip.SetActive(true);
    }

    // 툴팁 종료
    public void HideToolTip()
    {
        _goToolTip.SetActive(false);
    }


    // 구매 or 판매 의사 재질의
    public void BtnOK()
    {
        if (_isBuy)
        {
            if (_inven.GetGold() >= _price)
                _goRequestionUI.SetActive(true);
            else
                Debug.Log("골드가 부족합니다.");
        }
        else
            _goRequestionUI.SetActive(true);
    }

    // 승인
    public void BtnConfirm()
    {
        _goRequestionUI.SetActive(false);
        if (_isBuy) Buy();
        else Sell();
    }

    // 취소
    public void BtnCancel()
    {
        _goRequestionUI.SetActive(false);
    }

    // 판매 실행
    void Sell()
    {
        _inven.SetGold(_inven.GetGold() + _price);
        _inven.RemoveItem(_touchItem);
        HideToolTip();
        _shop.ReSetUI();
    }
    
    // 구매 실행
    void Buy()
    {
        _inven.SetGold(_inven.GetGold() - _price);
        _inven.TryToPushInventory(_touchItem);
        HideToolTip();
        _shop.ReSetUI();
    }
}