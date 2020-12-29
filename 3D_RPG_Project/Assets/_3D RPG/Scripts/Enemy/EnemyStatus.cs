using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    int id;

    [Header("Enemy Info")]
    [SerializeField] protected float _knockBackForce = 0.1f;
    [SerializeField] DropItemInfo[] _dropItemInfo = null;

    List<DropItem> _dropItemList = new List<DropItem>();

    bool _isDropItem = false;

    float removeTime = 5f;
    float curTime = 0f;

    public void Update()
    {
        if(_isDead && _dropItemList.Count < 1)
        {
            curTime += Time.deltaTime;
            if(curTime >= removeTime)
            {
                ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
            }
        }
    }

    protected override void Dead()
    {
        base.Dead();
        DesideDropItem();
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

    public int GetID() { return id; }

    protected override void HurtReaction(Vector3 targetPos)
    {
        // 필요시 넉백 구현
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();
        transform.position -= dir * _knockBackForce;
    }
}
