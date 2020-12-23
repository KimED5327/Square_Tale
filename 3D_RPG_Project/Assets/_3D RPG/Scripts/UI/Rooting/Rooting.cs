using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooting : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _goRootingUI = null;

    [Header("Slot")]
    [SerializeField] RootingSlot[] _rootingSlots = null;

    public static bool _isOpen = false;

    // 드롭 아이템
    List<DropItem> _rootingDropItem;

    // Component
    Inventory _inven;
    Shop _shop;
    EnemyStatus _rootingEnemy;
    
    void Awake()
    {
        _inven = FindObjectOfType<Inventory>();
        _shop = FindObjectOfType<Shop>();

        for (int i = 0; i < _rootingSlots.Length; i++)
            _rootingSlots[i].SetLink(this);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){

            if (_isOpen) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // 루팅 Enemy 정보 Get
                    _rootingEnemy = hit.transform.GetComponent<EnemyStatus>();

                    // Enemy가 죽은 경우에만 시도 
                    if (!_rootingEnemy.IsDead()) return;

                    // 드롭 아이템 정보 Get
                    _rootingDropItem = _rootingEnemy.GetDropItem();

                    if (_rootingDropItem != null && _rootingDropItem.Count > 0)
                    {
                        PushRootingSlot();
                        _isOpen = true;
                        _goRootingUI.SetActive(_isOpen);
                    }
                    else
                        _rootingEnemy = null;
                }
                else if (hit.transform.CompareTag("Npc"))
                {

                    if (!_isOpen)
                    {
                        _shop.CallMenu();
                        _isOpen = true;
                    }
                }
            }
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
                _rootingSlots[i].SetSlot(item, _rootingDropItem[i].itemCount, false);
            }
            else
                _rootingSlots[i].gameObject.SetActive(false);
        }
    }

    public void BtnExit()
    {
        // 남은 루팅 아이템, 다시 Enemy에게 넘겨줌.
        _rootingEnemy.SetDropItem(_rootingDropItem);
        _rootingEnemy = null;
        _rootingDropItem = null;

        _isOpen = false;
        _goRootingUI.SetActive(_isOpen);

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
