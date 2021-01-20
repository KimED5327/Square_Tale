using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    string _name = "필드 아이템";
    List<DropItem> _dropItemList = new List<DropItem>();

    [SerializeField] 
    bool _isAutoDestory = false;
    bool _isMustGetItem = false;
    bool _isDrop = false;

    float _removeTime = 60f;
    float _curTime = 0f;

    // 반드시 획득해야 되는 아이템이라면, itemChecker를 통해 획득했다는 정보를 전달한다.
    IMustNeedItem _itemChecker;
    [SerializeField] MeshRenderer _rend;

    //  아이템 초기화
    void DestroyFieldItem()
    {
        _isDrop = false;
        _dropItemList.Clear();
        _curTime = 0f;
        ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
    }

    // 일정 시간 지나면 아이템 제거
    public void Update()
    {
        if (_dropItemList.Count < 1)
        {
            if (_isMustGetItem) return;

            _curTime += Time.deltaTime;

            if (_curTime >= _removeTime)
            {
                DestroyFieldItem();
            }
        }
    }
    
    // 비활성화 되면 아이템 초기화
    private void OnDisable()
    {
        if(_isDrop)
            DestroyFieldItem();
    }

    // 드롭 아이템 정보 등록
    public void PushDropItem(List<DropItem> itemList, bool isGrahpicsShow = true, bool isMustGetItem = false, IMustNeedItem itemChecker = null)
    {
        _isDrop = true;
        _itemChecker = itemChecker;
        _dropItemList = itemList;
        _isMustGetItem = isMustGetItem;
        _rend.enabled = isGrahpicsShow;
    }

    // Get 드롭 아이템 정보
    public List<DropItem> GetDropItem()
    {
        return _dropItemList;
    }

    // 루팅 시도 후, 남은 드롭 아이템을 반영
    public void SetDropItem(List<DropItem> leftDropitem)
    {
        // 남은 것이 남아있으면 유지
        if (leftDropitem.Count > 0)
            _dropItemList = leftDropitem;

        // 남은 드롭 아이템이 없으면 초기화
        else
        {
            // 반드시 획득해야 하는 아이템이면 인터페이스를 통해 정보 전달 후 제거.
            if (_isMustGetItem && _itemChecker != null)
                _itemChecker.AcquireItem();

            DestroyFieldItem();
        }
        
    }

}
