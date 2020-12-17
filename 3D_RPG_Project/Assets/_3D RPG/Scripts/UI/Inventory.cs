﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private readonly int WEAPON = 0, ARMOR = 1, ETC = 2;

    [Header("OpenWindown")]
    [SerializeField] GameObject _goInventory = null;
    [SerializeField] GameObject _goEquip = null;
    bool _isOpen = false;

    [Header("Slots")]
    [SerializeField] GameObject _slotPrefab = null;
    [SerializeField] Transform _slotParent = null;
    [SerializeField] int _slotCount = 30;
    Slot[] _slots = null;

    [Header("Tab")]
    [SerializeField] Image[] imgTabs = null;
    [SerializeField] Color colorHighlight = new Color();
    [SerializeField] Color colorDark = new Color();
    int _currentTab = 0;

    #region Test
    // 테스트용 인벤 채우기
    void Start()
    {
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 무기"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 갑옷"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 잡화"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 무기"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 갑옷"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 잡화"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 무기"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 갑옷"));
        TryToPushInventory(ItemDatabase.instance.GetItem("테스트용 잡화"));

        // 기본값 - 무기 우선 정렬 
        OnTouchTab(WEAPON);
    }
    #endregion

    public int Gold { 
        get { 
            return Gold; 
        } 
        set { 
            Gold += value; 
            if (Gold < 0) Gold = 0; 
        } 
    }
    public int Ruby {
        get
        {
            return Ruby;
        }
        set
        {
            Ruby += value;
            if (Ruby < 0) Ruby = 0;
        }
    }

    private void Awake()
    {
        // 프리팹 슬롯 생성.
        _slots = new Slot[_slotCount];
        for (int i = 0; i < _slotCount; i++)
        {
            Slot slot = Instantiate(_slotPrefab, _slotParent).GetComponent<Slot>();
            _slots[i] = slot;
        }
    }


    // 인벤토리 버튼 터치
    public void OnTouchInventory()
    {
        _isOpen = !_isOpen;
        if (_isOpen) 
            ShowInven();
        else 
            HideInven();
    }

    // 열기
    void ShowInven()
    {
        _goInventory.SetActive(true);
        _goEquip.SetActive(true);
    }

    // 닫기
    void HideInven()
    {
        _goInventory.SetActive(false);
        _goEquip.SetActive(false);
    }

    // 정렬 탭 터치
    public void OnTouchTab(int index)
    {
        _currentTab = index;
      
        for(int i = 0; i < imgTabs.Length; i++)
            imgTabs[i].color = (i == _currentTab) ? colorHighlight: colorDark;

        SortItem((ItemType)_currentTab);
    }

    public void ResortItem() => SortItem((ItemType)_currentTab);

    public void SerializeItem()
    {
        // 슬롯 순회
        for(int i = 0; i < _slots.Length - 1; i++)
        {
            // 빈 슬롯이 있고,
            if (_slots[i].IsEmptySlot())
            {
                // 빈 슬롯 이후에 아이템 슬롯이 있다면, 앞으로 땡겨옴.
                int emptyIndex = i;
                for(int k = i + 1; k < _slots.Length; k++)
                {
                    if (!_slots[k].IsEmptySlot())
                    {
                        Item item = _slots[k].GetSlotItem();
                        int itemCount = _slots[k].GetSlotCount();
                        _slots[emptyIndex].ClearSlot();
                        _slots[emptyIndex++].PushSlot(item, itemCount);
                        _slots[k].ClearSlot();
                    }

                }
                return;
            }
        }
    }

    // 탭 우선 정렬
    void SortItem(ItemType type)
    {
        // 탭 우선 정렬로 인해 빠져나온 아이템 정보 저장용
        List<Item> popItemList = new List<Item>();
        List<int> popItemCountList = new List<int>();

        int headIndex = 0;
        for (int currentIndex = 0; currentIndex < _slots.Length; currentIndex++)
        {
            // 빈 슬롯을 만나면 정렬 종료
            if (_slots[currentIndex].IsEmptySlot()) break;

            // 해당 인덱스 슬롯에 담긴 아이템 정보를 얻어옴.
            Item item = _slots[currentIndex].GetSlotItem();
            int count = _slots[currentIndex].GetSlotCount();

            // 해당 아이템의 타입이 우선 정렬 대상이라면-
            if (item.type == type)
            {
                // 현재 인덱스와 헤드 인덱스가 같은 경우 헤드 인덱스만 이동. 
                if (currentIndex == headIndex)
                {
                    headIndex++;
                    continue; 
                }

                // 헤드 인덱스 슬롯에 아이템이 있다면
                if (!_slots[headIndex].IsEmptySlot())
                {
                    // 헤드 슬롯의 아이템을 빼내어 저장.
                    popItemList.Add(_slots[headIndex].GetSlotItem());
                    popItemCountList.Add(_slots[headIndex].GetSlotCount());
                }

                // 자기 자신은 클리어하고 헤드 슬롯에 덮어씌움.
                _slots[currentIndex].ClearSlot();
                _slots[headIndex++].PushSlot(item, count);
            }
        }

        // 이후 빠져나온 아이템 수만큼 빈슬롯에 차례대로 저장.
        int listCount = 0;
        for (int currentIndex = 0; currentIndex < _slots.Length; currentIndex++)
        {
            // 빠져나온 아이템들을 다시 집어넣으면 종료
            if (listCount >= popItemCountList.Count) break;

            // 빠져나온 아이템들은 빈슬롯과 만나면 저장시킴
            if (_slots[currentIndex].IsEmptySlot())
            {
                _slots[currentIndex].PushSlot(popItemList[listCount], popItemCountList[listCount]);
                listCount++;
            }
            
        }
    }

    // true : 획득 성공, false : 획득 실패 (빈 슬롯 부족 등등)
    public bool TryToPushInventory(Item item, int count = 1)
    {

        // 빈 슬롯이 있다면-
        if (CheckIsEmptySlot())
        {
            // 중첩 가능한 아이템-
            if (item.type == ItemType.ETC)
            {
                // 소유한 아이템 -> 해당 슬롯의 개수 증가 else 빈 슬롯에 푸시
                if (TryToPushSameSlot(item, count))
                    return true;
                else
                {
                    if (TryToPushAnySlot(item, count))
                        return true;
                }

            }
            // 중첩 불가능한-
            else
            {
                count = 1;
                // 빈 인벤토리 슬롯에 푸시.
                if (TryToPushAnySlot(item, count))
                    return true;
            }
        }

        Debug.Log("아이템 획득 실패!");
        Debug.Log("적절한 빈 슬롯이 없습니다!");

        return false;
    }



    // 해당 인덱스 아이템 슬롯 제거
    public void RemoveItem(int index)
    {
        if (_slots[index].IsEmptySlot())
            Debug.Log("빈 슬롯입니다.");
        else
            _slots[index].ClearSlot();
    }

    // 해당 아이템 슬롯 제거
    public void RemoveItem(Item item)
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                _slots[i].ClearSlot();
                return;
            }
        }
    }

    bool TryToPushSameSlot(Item item, int count)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                _slots[i].IncreaseSlotCount(count);
                return true;
            }
        }
        return false;
    }

    bool TryToPushAnySlot(Item item, int count)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
            {
                _slots[i].PushSlot(item, count);
                return true;
            }
        }
        return false;
    }

    bool CheckIsEmptySlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
            {
                return true;
            }
        }
        return false;
    }

    public Item GetSlotItem(int index) { return _slots[index].GetSlotItem(); }
    public Vector3 GetSlotLocalPos(int index) { return _slots[index].transform.localPosition; }
}
