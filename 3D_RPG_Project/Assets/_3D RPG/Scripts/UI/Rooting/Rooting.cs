using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooting : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _goRootingUI = null;

    [Header("Slot")]
    [SerializeField] RootingSlot[] _rootingSlots = null;

    // 드롭 아이템
    List<DropItem> _rootingDropItem;

    // Component
    Inventory _inven;
    FieldItem _rootingItem;
    
    void Awake()
    {
        _inven = FindObjectOfType<Inventory>();

        for (int i = 0; i < _rootingSlots.Length; i++)
            _rootingSlots[i].SetLink(this);
    }

    public bool TryRooting(FieldItem fieldItem)
    {
        _rootingItem = fieldItem;

        // 드롭 아이템 정보 Get
        _rootingDropItem = _rootingItem.GetDropItem();

        if (_rootingDropItem != null && _rootingDropItem.Count > 0)
        {
            PushRootingSlot();
            _goRootingUI.SetActive(true);
            return true;
        }
        else
        { 
            _rootingItem = null;
            return false;
        }
    }

    void PushRootingSlot()
    {
        // 드롭 아이템 개수만큼 루팅 슬롯에 푸시
        for (int i = 0; i < _rootingSlots.Length; i++)
        {
            if (i < _rootingDropItem.Count)
            {
                _rootingSlots[i].gameObject.SetActive(true);
                Item item = ItemDatabase.instance.GetItem(_rootingDropItem[i].itemID);
                _rootingSlots[i].SetSlot(item, _rootingDropItem[i].itemCount);
            }
            else
                _rootingSlots[i].gameObject.SetActive(false);
        }

        if (_rootingDropItem.Count == 0)
            StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {

        yield return new WaitForSeconds(0.15f);
        BtnExit();
    }

    public void BtnExit()
    {
        // 남은 루팅 아이템, 다시 Enemy에게 넘겨줌.
        _rootingItem.SetDropItem(_rootingDropItem);
        _rootingItem = null;
        _rootingDropItem = null;
        _inven.SaveInventory();
        InteractionManager._isOpen = false;
        _goRootingUI.SetActive(false);
    }

    public void PushInventory(Item item, int count)
    {
        if(!_inven.TryToPushInventory(item, count))
            Debug.Log("루팅 실패");
        else
            Resort(item.id);
    }

    void Resort(int itemId)
    {
        // 터치해서 습득한 루팅 슬롯은 제거
        for (int i = 0; i < _rootingDropItem.Count; i++)
        {
            if (_rootingDropItem[i].itemID == itemId)
            {
                _rootingDropItem.RemoveAt(i);
                break;
            }
        }

        // 다시 정렬
        PushRootingSlot();
    }
}
