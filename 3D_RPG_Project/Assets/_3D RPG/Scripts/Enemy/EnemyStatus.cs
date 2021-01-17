using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    int id;

    [Header("Enemy Info")]
    [SerializeField] protected float _knockBackForce = 0.25f;
    [SerializeField] protected bool _canKnockBack = true;
    [SerializeField] DropItemInfo[] _dropItemInfo = null;
    [SerializeField] protected bool _isForceDead = false; // 스폰과 동시에 루팅 가능하도록 세팅된 Enemy (혹은 오브젝트)

    List<DropItem> _dropItemList = new List<DropItem>();

    bool _isDropItem = false;

    float removeTime = 5f;
    float curTime = 0f;

    public override void Initialized()
    {
        base.Initialized();
        curTime = 0f;
        _isDropItem = false;
        _dropItemList.Clear();

        // 스폰과 동시에 루팅 가능하도록 세팅된 Enemy (혹은 오브젝트)
        // 정화의 크리스탈을 부수면, 크리스탈 이펙트가 스폰됨. 
        // 이때, 그 이펙트를 즉시 die 처리해서 루팅할 수 있게 만드는 과정임.
        if (_isForceDead)
            Dead();
    }

    public void Update()
    {
        if(_dropItemList.Count < 1)
        {
            if (_isDead)
            {
                curTime += Time.deltaTime;

                // 처음부터 루팅 가능하도록 세팅된 Enemy (혹은 오브젝트)
                if (_isForceDead)
                    GetComponent<CrystalCheck>().AcquireCrystal();
                
                if (curTime >= removeTime)
                {
                    if (!_isForceDead)
                        ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);

                }
            }

        }
    }

    protected override void Dead()
    {
        base.Dead();

        // 현재 '몬스터 처치' 퀘스트를 진행 중이라면, 퀘스트 달성 조건 검사 
        if(!_isForceDead)
            QuestManager.instance.CheckKillEnemyQuest(GetComponent<Enemy>().Id);

        DecideDropItem();
    }


    // 죽으면 실행
    public void DecideDropItem()
    {
        _dropItemList.Clear();

        if (!_isForceDead)
        {
            DropItem item = new DropItem { itemID = 1, itemCount = Random.Range(2, 10) };
            _dropItemList.Add(item);
        }


        // 각 아이템별 랜덤 드롭 확률에 걸리면 드롭 리스트에 추가
        for (int i = 0; i < _dropItemInfo.Length; i++)
        {
            float random = Random.Range(0f, 100f);

            // 강제로 드롭 아이템을 떨구는 경우 +
            if (random <= _dropItemInfo[i].itemDropRate || _isForceDead)
            {
                DropItem dropItem = new DropItem
                {
                    itemID = _dropItemInfo[i].itemID,
                    itemCount = Random.Range(_dropItemInfo[i].itemCounts.min, _dropItemInfo[i].itemCounts.max + 1)
                };
                _dropItemList.Add(dropItem);
            }
        }

        // 드롭 리스트에 하나라도 있으면 드롭 OK
        if (_dropItemList.Count > 0)
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

    public int GetID() { return id; }

    protected override void HurtReaction(Vector3 targetPos)
    {
        // 필요시 넉백 구현
        if (!_canKnockBack) return;

        Vector3 dir = targetPos - transform.position;
        dir.Normalize();
        transform.position -= dir * _knockBackForce;
    }
}
