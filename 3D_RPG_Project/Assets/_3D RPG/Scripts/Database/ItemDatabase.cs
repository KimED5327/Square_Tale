using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 테스트 목적. 이후 JSON -> 테이블 로더를 통해 재구축.
    // Dictionary로 변경 예정.
    [SerializeField] Item[] items = null;

    public static ItemDatabase instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public Item GetItem(string itemName)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].name == itemName)
            {
                return items[i];
            }
        }
        return null;
    }
}
