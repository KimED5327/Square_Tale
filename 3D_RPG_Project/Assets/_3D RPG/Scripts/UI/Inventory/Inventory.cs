using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int gold = 50;
    int ruby = 0;

    [Header("OpenWindown")]
    [SerializeField] GameObject _goInventory = null;
    [SerializeField] GameObject _goEquip = null;
    [SerializeField] GameObject _goBackButton = null;
    [SerializeField] GameObject _goBackground = null;

    bool _isOpen = false;

    [Header("Slots")]
    [SerializeField] GameObject _slotPrefab = null;
    [SerializeField] Transform _slotParent = null;
    [SerializeField] int _slotCount = 30;
    Slot[] _slots = null;

    [Header("Tab")]
    [SerializeField] GameObject _goInvenButtons = null;
    [SerializeField] Image[] _imgTabs = null;
    int _currentTab = 0;


    [Header("ShopOffset")]
    [SerializeField] Transform _tfOffset = null;
    [SerializeField] float _offsetX = 100;
    Vector3 _originPos;
    Vector3 _shopPos;


    Tutorial _tutorial;


    #region Test
    // 테스트용 인벤 채우기
    void Start()
    { 
        // 기본값 - 무기 우선 정렬 
        OnTouchTab(0);


        LoadInventory();

    }
    #endregion

    public void LoadInventory()
    {
        for (int i = 0; i < _slots.Length; i++)
        {

            if (!PlayerPrefs.HasKey("InvenSlotItemId" + i))
            {
                SaveInventory();
                return;
            }
            else
            {
                int id = PlayerPrefs.GetInt("InvenSlotItemId" + i);
                if(id != 0)
                {
                    _slots[i].PushSlot(
                        ItemDatabase.instance.GetItem(
                            PlayerPrefs.GetInt("InvenSlotItemId" + i))
                        ,
                        PlayerPrefs.GetInt("InvenSlotItemCount" + i)
                    );
                }
            }
        }
        gold = PlayerPrefs.GetInt("Gold");
    }

    // dkdlxp
    public void SaveInventory()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
            {
                PlayerPrefs.SetInt("InvenSlotItemId" + i, 0);
                PlayerPrefs.SetInt("InvenSlotItemCount" + i, 0);
                PlayerPrefs.SetInt("Gold", gold);
            }
            else
            {
                Item item = _slots[i].GetSlotItem();
                PlayerPrefs.SetInt("InvenSlotItemId" + i, item.id);
                PlayerPrefs.SetInt("InvenSlotItemCount" + i, _slots[i].GetSlotCount());
                PlayerPrefs.SetInt("Gold", gold);
            }
        }
    }

    private void Awake()
    {
        _tutorial = FindObjectOfType<Tutorial>();

        _originPos = _goInventory.transform.localPosition;
        _shopPos = _tfOffset.localPosition + new Vector3(_offsetX, 0, 0); 

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
    public void ShowInven(bool isShopOpen = false)
    {
        SoundManager.instance.PlayEffectSound("PopUp");

        GameHudMenu.instance.HideMenu(); // 기본 HUD 가림

        // 상점에서 연 경우, 위치 이동 + 탭 숨김.
        _goInventory.transform.localPosition = isShopOpen ? _shopPos : _originPos;
        _goInvenButtons.SetActive(!isShopOpen);
        _goEquip.SetActive(!isShopOpen);
        _goBackButton.SetActive(!isShopOpen);
        _goBackground.SetActive(!isShopOpen);

        // 튜토리얼 호출
        if (!isShopOpen)
            _tutorial.CallTutorial(TutorialType.INVENTORY);


        _goInventory.SetActive(true);
    }

    // 닫기
    public void HideInven(bool hideMenu = true)
    {
        if (hideMenu)
        {
            GameHudMenu.instance.ShowMenu();
            SoundManager.instance.PlayEffectSound("PopDown");
        }
        SlotToolTip.instance.HideToolTip();
        _goInventory.SetActive(false);
        _goEquip.SetActive(false);
    }

    // 정렬 탭 터치
    public void OnTouchTab(int index)
    {
        SoundManager.instance.PlayEffectSound("Click");
        SlotToolTip.instance.HideToolTip();

        _currentTab = index;
      
        for(int i = 0; i < _imgTabs.Length; i++)
            _imgTabs[i].color = (i == _currentTab) ? Color.white : Color.gray;

        SortItem((ItemCategory)_currentTab);
    }

    public void ResortItem()
    {
        SerializeItem();
        SortItem((ItemCategory)_currentTab);
    }

    // 재정렬
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
    public void SortItem(ItemCategory category)
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
            if (item.category == category)
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

    /// <summary>
    /// 아이템을 인벤에 넣는 시도를 함. true면 성공, false는 실패
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool TryToPushInventory(Item item, int count = 1)
    {

        // id == 1 은 골드
        if (item.id == 1)
        {
            gold += count;
            return true;
        }

        // 빈 슬롯이 있다면-
        if (CheckIsEmptySlot())
        {
            // 중첩 가능한 아이템-
            if (item.stackable)
            {
                // 소유한 아이템 -> 해당 슬롯의 개수 증가 else 빈 슬롯에 푸시
                if (TryToPushSameSlot(item, count))
                    return true;
                else
                {
                    if (TryToPushEmptySlot(item, count))
                        return true;
                }

            }
            // 중첩 불가능한-
            else
            {
                count = 1;
                // 빈 인벤토리 슬롯에 푸시.
                if (TryToPushEmptySlot(item, count))
                    return true;
            }
        }

        Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughInventory);

        return false;
    }

    /// <summary>
    /// Item ID로 Push 시도 가능
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool TryToPushInventory(int itemID, int count = 1)
    {
        Item item = ItemDatabase.instance.GetItem(itemID);

        // id == 1 은 골드
        if (item.id == 1)
        {
            gold += count;
            return true;
        }

        // 빈 슬롯이 있다면-
        if (CheckIsEmptySlot())
        {
            // 중첩 가능한 아이템-
            if (item.stackable)
            {
                // 소유한 아이템 -> 해당 슬롯의 개수 증가 else 빈 슬롯에 푸시
                if (TryToPushSameSlot(item, count))
                    return true;
                else
                {
                    if (TryToPushEmptySlot(item, count))
                        return true;
                }

            }
            // 중첩 불가능한-
            else
            {
                count = 1;
                // 빈 인벤토리 슬롯에 푸시.
                if (TryToPushEmptySlot(item, count))
                    return true;
            }
        }

        Notification.instance.ShowFloatingMessage(StringManager.msgNotEnoughInventory);

        return false;
    }



    // 해당 인덱스 아이템 슬롯 제거
    public void RemoveItemIndex(int index)
    {
        if (_slots[index].IsEmptySlot())
            Debug.Log("빈 슬롯입니다.");
        else
        {
            _slots[index].ClearSlot();
            SerializeItem();
        }
    }

    /// <summary>
    /// 해당 아이템 슬롯 제거
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                _slots[i].ClearSlot();
                SerializeItem();
                return;
            }
        }
    }
    /// <summary>
    /// Item ID로 슬롯 제거 시도
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(int itemID)
    {
        Item item = ItemDatabase.instance.GetItem(itemID);

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                _slots[i].ClearSlot();
                SerializeItem();
                return;
            }
        }
    }

    /// <summary>
    /// 아이템 감소
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void DecreaseItemCount(Item item, int count)
    {

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                count = _slots[i].DecreaseCount(count);
                if (count <= 0)
                    break;

            }
        }

        SerializeItem();
    }

    public void DecreaseItemCount(int itemID, int count)
    {
        Item item = ItemDatabase.instance.GetItem(itemID);

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsSameItem(item))
            {
                count = _slots[i].DecreaseCount(count);
                if (count <= 0)
                    break;

            }
        }

        SerializeItem();
    }

    // 같은 이름의 아이템에 푸시
    bool TryToPushSameSlot(Item item, int count)
    {
        int originSlotIndex;
        int originCount;
        for (int i = 0; i < _slots.Length; i++)
        {
            // 같은 아이템이 있다면 개수 증가.
            // 이미 99개 꽉차있다면 새로운 빈슬롯에 추가.
            if (!_slots[i].IsEmptySlot() && _slots[i].IsSameItem(item))
            {
                // 넘쳐서 남은 개수를 추가로 넣을 빈 슬롯이 없을 경우를 대비.
                originCount = _slots[i].GetSlotCount();
                originSlotIndex = i;

                // 슬롯 개수 증가. 넘쳐서 남은 값 리턴.
                count = _slots[i].IncreaseSlotCount(count);

                // 넘쳐서 남았다면...
                if (count > 0)
                {
                    // 빈 슬롯을 찾아서 남은 개수 푸시.
                    if (!TryToPushEmptySlot(item, count))
                    {
                        _slots[originSlotIndex].ForceSetSlotCount(originCount);
                        return false;
                    }
                }

                // ※ 퀘스트 조건검사 
                // 진행 중인 퀘스트 중 '아이템 전달' 퀘스트가 있다면 인벤토리에 해당 아이템이 있는지 달성 요건 확인 
                QuestManager.instance.CheckDeliverItemQuest();
                // 진행 중인 퀘스트 중 '아이템 소지' 퀘스트가 있다면 인벤토리에 해당 아이템이 있는지 달성 요건 확인 
                QuestManager.instance.CheckCarryItemQuest();
                return true;
            }
        }
        return false;
    }

    // 빈 슬롯에 푸시
    bool TryToPushEmptySlot(Item item, int count)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
            {
                _slots[i].PushSlot(item, count);

                // ※ 퀘스트 조건검사
                // 진행 중인 퀘스트 중 '아이템 전달' 퀘스트가 있다면 인벤토리에 해당 아이템이 있는지 달성 요건 확인 
                QuestManager.instance.CheckDeliverItemQuest();
                // 진행 중인 퀘스트 중 '아이템 소지' 퀘스트가 있다면 인벤토리에 해당 아이템이 있는지 달성 요건 확인 
                QuestManager.instance.CheckCarryItemQuest();
                return true;
            }
        }
        return false;
    }

    // 빈슬롯 체크
    bool CheckIsEmptySlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
                return true;
        }
        return false;
    }

    // 빈슬롯이 몇개인지 체크
    public int GetEmptySlotCount()
    {
        int count = 0;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmptySlot())
                count++;
        }
        return count;
    }

    
    /// 아이템을 특정 수량만큼 가지고 있는지 체크.
    public bool HaveItemCount(Item item, int count)
    {
        int total = 0;
        for(int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmptySlot())
            {
                if (item.id == _slots[i].GetSlotItem().id)
                {
                    total += _slots[i].GetSlotCount();
                }
            }
        }
        return count <= total;
    }

    public int GetItemCount(Item item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmptySlot())
            {
                if (item.id == _slots[i].GetSlotItem().id)
                {
                    return _slots[i].GetSlotCount();
                }
            }
        }

        return 0;
    }
    /// <summary>
    /// item ID 로 개수 카운트
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int GetItemCount(int itemID)
    {
        Item item = ItemDatabase.instance.GetItem(itemID);

        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmptySlot())
            {
                if (item.id == _slots[i].GetSlotItem().id)
                {
                    return _slots[i].GetSlotCount();
                }
            }
        }

        return 0;
    }


    public Item GetSlotItem(int index) { return _slots[index].GetSlotItem(); }
    public Vector3 GetSlotLocalPos(int index) { return _slots[index].transform.localPosition; }

    public int GetGold() { return gold; }
    public int GetRuby() { return ruby; }
    public void SetGold(int num) { gold = num; if (gold < 0) gold = 0; SaveInventory(); }
    public void SetRuby(int num) { ruby = num; if (ruby < 0) ruby = 0; }


}
