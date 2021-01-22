using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 테스트 목적. 이후 JSON -> 테이블 로더를 통해 재구축.
    // Dictionary로 변경 예정.
    //[SerializeField] Item[] items = null; // 임시 테이블
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


    public Item GetItem(int itemID)
    {
        if (!itemDB.ContainsKey(itemID))
            Debug.LogError(itemID + " 은 등록된 아이템이 아닙니다.");

        return itemDB[itemID];
    }

}
