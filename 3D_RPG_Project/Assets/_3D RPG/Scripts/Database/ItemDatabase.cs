using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
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
