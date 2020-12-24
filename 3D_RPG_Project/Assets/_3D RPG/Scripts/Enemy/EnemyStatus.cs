using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    [SerializeField]
    DropItemInfo[] _dropItemInfo;

    List<DropItem> _dropItemList = new List<DropItem>();

    bool _isDropItem = false;

    public void Update()
    {
        // 임시. 죽으면 DesideDropItem을 호출한 이후에 지울 것.
        if (!_isDead)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("몬스터 사망했으니, 시체를 터치하여 아이템 획득을 시도하세요.");
                _isDead = true;
                DesideDropItem();
                Debug.Log("이번에 드롭된 아이템 수는 = " + _dropItemList.Count);
            }
        }

    }



    // 죽으면 실행
    public void DesideDropItem()
    {
        // 각 아이템별 랜덤 드롭 확률에 걸리면 드롭 리스트에 추가
        for(int i = 0; i < _dropItemInfo.Length; i++)
        {
            float random = Random.Range(0f, 100f);
            if(random <= _dropItemInfo[i].itemDropRate)
            {
                DropItem dropItem = new DropItem{
                    itemID = _dropItemInfo[i].itemID,
                    itemCount = Random.Range(_dropItemInfo[i].itemCounts.min, _dropItemInfo[i].itemCounts.max + 1)
                };
                _dropItemList.Add(dropItem);
            }
        }

        // 드롭 리스트에 하나라도 있으면 드롭 OK
        if(_dropItemList.Count > 0)
        {
            _isDropItem = true;
        }
    }

    public List<DropItem> GetDropItem()
    {
        if (!_isDropItem)
            return null;

        return _dropItemList;
    }


    public void SetDropItem(List<DropItem> leftDropitem) 
    {
        if (leftDropitem.Count > 0)
            _dropItemList = leftDropitem;
        else
        {
            _dropItemList.Clear();
            _isDropItem = false;
        }
    }

    //public void Initialized()
    //{
    //    _isDropItem = false;
    //    _dropItemList.Clear();
    //}
}
