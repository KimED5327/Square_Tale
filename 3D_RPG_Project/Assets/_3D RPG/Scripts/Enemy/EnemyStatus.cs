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

    [SerializeField] float _removeTime = 5f;
    [SerializeField] float _respawnTime = 20f;
    float _curTime = 0f;

    SkinnedMeshRenderer _render;

    public override void Initialized()
    {
        base.Initialized();
        _curTime = 0f;

        if (!_isForceDead)
        {
            if (_render == null)
                _render = GetComponentInChildren<SkinnedMeshRenderer>();
            
            _render.enabled = true;
        }

        // 스폰과 동시에 루팅 가능하도록 세팅된 Enemy (혹은 오브젝트)
        // 정화의 크리스탈을 부수면, 크리스탈 이펙트가 스폰됨. 
        // 이때, 그 이펙트를 즉시 die 처리해서 루팅할 수 있게 만드는 과정임.
        if (_isForceDead)
            Dead();
    }

    public void Update()
    {
        if (_isDead)
        {
            _curTime += Time.deltaTime;

            // 일정 시간 이후 시체만 사라지게 (= 렌더가 안 되게)
            if (_curTime >= _removeTime)
            {
                if (!_isForceDead)
                    _render.enabled = false;
            }

            // 리스폰 시스템이 캐치하도록 오브젝트 반납
            if(_curTime >= _respawnTime)
            {
                if (!_isForceDead)
                    ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
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
        List<DropItem> dropItemList = new List<DropItem>();

        if (!_isForceDead)
        {
            DropItem item = new DropItem { itemID = 1, itemCount = _giveGold };
            dropItemList.Add(item);
        }

        // 각 아이템별 랜덤 드롭 확률에 걸리면 드롭 리스트에 추가
        for (int i = 0; i < _dropItemInfo.Length; i++)
        {
            float random = Random.Range(0f, 100f);

            // 확률 계산 or 강제 드롭... 을 통해 드롭 아이템 결정
            if (random <= _dropItemInfo[i].itemDropRate || _isForceDead)
            {
                DropItem dropItem = new DropItem
                {
                    itemID = _dropItemInfo[i].itemID,
                    itemCount = Random.Range(_dropItemInfo[i].itemCounts.min, _dropItemInfo[i].itemCounts.max + 1)
                };
                dropItemList.Add(dropItem);
            }
        }

        // 드롭 리스트에 하나라도 있으면 드롭 OK
        if (dropItemList.Count > 0)
        {
            Debug.Log(ObjectPooling.instance);
            GameObject go = ObjectPooling.instance.GetObjectFromPool("필드 아이템", transform.position);
            FieldItem fieldItem = go.GetComponent<FieldItem>();

            // 드롭 아이템은 일정한 거리의 랜덤한 위치에서
            if (!_isForceDead)
            {
                float randomPos = 0.5f;
                randomPos = Random.Range(0, 100) > 50 ? randomPos : -randomPos;
                go.transform.position += new Vector3(randomPos, 0f, randomPos);
                fieldItem.PushDropItem(dropItemList);
            }
            else
                fieldItem.PushDropItem(dropItemList, false, true, GetComponent<IMustNeedItem>());
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
