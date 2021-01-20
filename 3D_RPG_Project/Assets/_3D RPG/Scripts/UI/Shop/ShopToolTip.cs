using System.Collections;
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
    [SerializeField] Text _txtCount = null;

    [Header("Button")]
    [SerializeField] Text _txtButton = null;


    [Header("Requestion")]
    [SerializeField] GameObject _goRequestionUI = null;
    [SerializeField] Text _txtRequestion = null;


    Item _touchItem;
    int _count;
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
        SoundManager.instance.PlayEffectSound("Click");
        _isBuy = isBuy;
        _touchItem = item;
        _price = isBuy ? item.priceBuy : item.priceSell;
        _count = 1;

        // 돈없으면 구매창 출력 X
        if(isBuy && _price > _inven.GetGold())
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughGold);
            return;
        }

        // 툴팁 내용 세팅
        _txtName.text = item.name;
        _txtDesc.text = item.desc;
        _imgIcon.sprite = SpriteManager.instance.GetItemSprite(item.id);
        //_imgIcon.sprite = item.sprite;
        _txtCount.text = _count.ToString();
        _txtPrice.text = string.Format("{0:#,##0}", _price);
        if (isBuy)
            _txtPrice.color = (_price > _inven.GetGold()) ? Color.red : Color.white;
        else
            _txtPrice.color = Color.white;

        _txtOption.text = "";
        if (item.options.Count > 0)
        {
            for (int i = 0; i < item.options.Count; i++)
                _txtOption.text += StringManager.ItemTypeToString(item.options[i].opType) + " " + item.options[i].num + " ";
        }

        _txtTitle.text = (_isBuy) ? StringManager.shopObjectBuy : StringManager.shopObjectSell;
        _txtButton.text = (_isBuy) ? StringManager.shopBuy : StringManager.shopSell;
        _txtRequestion.text = (_isBuy) ? StringManager.shopBuyMessage
                                       : StringManager.shopSellMessage;

        _goToolTip.SetActive(true);
    }

    // 툴팁 종료
    public void HideToolTip()
    {
        SoundManager.instance.PlayEffectSound("Click");
        _goToolTip.SetActive(false);
    }


    public void BtnIncreaseCount()
    {
        SoundManager.instance.PlayEffectSound("Click");
        // 팔때 가격 살때 가격 구분 필요
        if (_count < 99)
        {
            // 장비류는 살때와 팔때 모두 1개씩만 가능
            if (_touchItem.category != ItemCategory.ETC) 
                return;

            // 판매할 때는 소유한 개수까지만 개수 증가 가능.            
            if (!_isBuy && _inven.GetItemCount(_touchItem) <= _count)
                return;
            
            _count++;    

            if (_isBuy)
                _price += _touchItem.priceBuy;
            else
                _price += _touchItem.priceSell;

            ShowPriceAndCount();
        }
    }
    public void BtnDecreaseCount()
    {
        SoundManager.instance.PlayEffectSound("Click");
        if (_count > 1) {
            _count -= 1;
            if (_isBuy)
                _price -= _touchItem.priceBuy;
            else
                _price -= _touchItem.priceSell;

            ShowPriceAndCount();
        }
    }
    public void BtnMaxCount()
    {
        SoundManager.instance.PlayEffectSound("Click");

        // 장비류는 살때와 팔때 모두 1개씩만 가능
        if (_touchItem.category != ItemCategory.ETC)
            return;

        if (_count < 99)
        {
            if (_isBuy)
                _count = _inven.GetGold() / _touchItem.priceBuy;
            else
                _count = _inven.GetItemCount(_touchItem);
            
            if (_count > 99)
                _count = 99;

            _price = (_isBuy) ? _touchItem.priceBuy * _count : _touchItem.priceSell * _count;


            ShowPriceAndCount();   
        }
    }
    public void BtnMinCount()
    {
        SoundManager.instance.PlayEffectSound("Click");

        _count = 1;
        _price = (_isBuy) ? _touchItem.priceBuy : _touchItem.priceSell;

        ShowPriceAndCount();
    }

    void ShowPriceAndCount()
    {
        _txtPrice.text = string.Format("{0:#,##0}", _price);
        if (_isBuy)
            _txtPrice.color = (_price > _inven.GetGold()) ? Color.red : Color.white;
        else
            _txtPrice.color = Color.white;
        _txtCount.text = _count.ToString();
    }

    // 구매 or 판매 의사 재질의
    public void BtnOK()
    {
        SoundManager.instance.PlayEffectSound("Click");
        if (_isBuy)
        {
            if (_inven.GetGold() >= _price)
                _goRequestionUI.SetActive(true);
            else
                Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughGold);
        }
        else
        {
            if (_inven.GetItemCount(_touchItem) < _count)
                Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughItemCount);
            else
               _goRequestionUI.SetActive(true);
        }

    }

    // 승인
    public void BtnConfirm()
    {
        SoundManager.instance.PlayEffectSound("Click");
        _goRequestionUI.SetActive(false);
        if (_isBuy) Buy();
        else Sell();
    }

    // 취소
    public void BtnCancel()
    {
        SoundManager.instance.PlayEffectSound("Click");
        _goRequestionUI.SetActive(false);
    }

    // 판매 실행
    void Sell()
    {
        _inven.SetGold(_inven.GetGold() + _price);
        _inven.DecreaseItemCount(_touchItem, _count);
        FinishProcess();

        // 퀘스트 조건검사 
        QuestManager.instance.CheckDeliverItemQuest();
        QuestManager.instance.CheckCarryItemQuest();
    }
    
    // 구매 실행
    void Buy()
    {
        _inven.SetGold(_inven.GetGold() - _price);
        _inven.TryToPushInventory(_touchItem, _count);
        FinishProcess();
    }

    // 마무리
    void FinishProcess()
    {
        SoundManager.instance.PlayEffectSound("Coin");
        HideToolTip();
        _shop.ReSetUI();
        _inven.SaveInventory();
    }
}
