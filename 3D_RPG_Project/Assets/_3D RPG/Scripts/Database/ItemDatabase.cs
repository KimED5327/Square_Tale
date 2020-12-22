using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 테스트 목적. 이후 JSON -> 테이블 로더를 통해 재구축.
    // Dictionary로 변경 예정.
    [SerializeField] Item[] items = null; // 임시 테이블
    Dictionary<int, Item> itemDB = new Dictionary<int, Item>(); // 실 활용 테이블


    public static ItemDatabase instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void AddItem(Item item, int itemId)
    {
        itemDB.Add(itemId, item);
    }

    // 임시
    public Item GetItem(int itemId)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].id == itemId)
            {
                return items[i];
            }
        }
        return null;
    }

    // 실제 사용
    //public Item GetItem(string itemName)
    //{
    //    Item item = itemDB[itemName];
    //    if (item == null)
    //        Debug.Log(itemName + " 은/는 등록된 아이템이 아닙니다.");

    //    return item;
    //}

}
