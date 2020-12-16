using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private readonly int WEAPON = 0, ARMOR = 1, ETC = 2;

    [Header("Slots")]
    [SerializeField] GameObject _goInventory = null;
    [SerializeField] GameObject _slotPrefab = null;
    [SerializeField] Transform _slotParent = null;
    [SerializeField] int _slotCount = 30;

    Slot[] _slots = null;

    [Header("Tab")]
    [SerializeField] Image[] imgTabs = null;
    [SerializeField] Color colorHighlight = new Color();
    [SerializeField] Color colorDark = new Color();


    bool _isOpen = false;
    int _currentTab = 0;

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

    // 골드
    public int Gold { 
        get { 
            return Gold; 
        } 
        set { 
            Gold += value; 
            if (Gold < 0) Gold = 0; 
        } 
    }

    // 캐시
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
        _slots = new Slot[_slotCount];
        for (int i = 0; i < _slotCount; i++)
        {
            Slot slot = Instantiate(_slotPrefab, _slotParent).GetComponent<Slot>();
            slot.SetSlotIndex(i);
            _slots[i] = slot;
        }
    }


    // 인벤토리 버튼
    void OnTouchInventory()
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
    }

    // 닫기
    void HideInven()
    {
        _goInventory.SetActive(false);
    }

    // 정렬 탭 터치
    public void OnTouchTab(int index)
    {
        _currentTab = index;
      
        for(int i = 0; i < imgTabs.Length; i++)
            imgTabs[i].color = (i == _currentTab) ? colorHighlight: colorDark;

        if (_currentTab == WEAPON)
            SortItem(ItemType.WEAPON);
        else if (_currentTab == ARMOR)
            SortItem(ItemType.ARMOR);
        else if (_currentTab == ETC)
            SortItem(ItemType.ETC);
    }

    // 탭 우선 정렬
    void SortItem(ItemType type)
    {
        // 우선 정렬로 인해 빠져나온 아이템 정보 저장용
        List<Item> popItemList = new List<Item>();
        List<int> popItemCountList = new List<int>();


        int headCount = 0;
        int currentIndex = 0;
        for (; currentIndex < _slots.Length; currentIndex++)
        {
            // 빈 슬롯을 만나면 정렬 종료
            if (_slots[currentIndex].IsEmptySlot()) break;

            // 해당 인덱스 아이템 정보 Get
            Item item = _slots[currentIndex].GetSlotItem();
            int count = _slots[currentIndex].GetSlotCount();

            // 해당 아이템의 타입이 우선 정렬 대상이라면-
            if (item.type == type)
            {
                // 인덱스가 같은 경우 헤드카운트만 증가하고 무시.
                if (currentIndex == headCount)
                {
                    headCount++;
                    continue; 
                }

                // 헤드 슬롯에 아이템이 있다면
                if (!_slots[headCount].IsEmptySlot())
                {
                    // 헤드 슬롯의 아이템을 빼내어 저장하고 그 자리로 덮어씌움.
                    popItemList.Add(_slots[headCount].GetSlotItem());
                    popItemCountList.Add(_slots[headCount].GetSlotCount());
                }

                _slots[currentIndex].ClearSlot();
                _slots[headCount++].PushSlot(item, count);
            }
        }

        // 이후 빠져나온 아이템 수만큼 빈슬롯에 차례대로 저장.
        int listCount = 0;
        for (currentIndex = 0; currentIndex < _slots.Length; currentIndex++)
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
                // 빈 인벤토리 슬롯에 푸시.
                if (TryToPushAnySlot(item, 1))
                    return true;
            }
        }

        Debug.Log("아이템 획득 실패!");
        Debug.Log("적절한 빈 슬롯이 없습니다!");

        return false;
    }




    public void DropItem(int index)
    {
        if (_slots[index].IsEmptySlot())
            Debug.Log("빈 슬롯입니다.");
        else
            _slots[index].ClearSlot();
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
