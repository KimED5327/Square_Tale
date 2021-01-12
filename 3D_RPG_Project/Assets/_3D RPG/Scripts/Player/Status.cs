using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    [Header("Basic Status")]
    [SerializeField] protected string _name = "이름";
    [SerializeField] protected int _maxHp = 100;
    protected int _curHp = 100;
    [SerializeField] protected int _atk = 0;
    [SerializeField] protected int _def = 0;
    [SerializeField] protected int _level = 1;
    [SerializeField] protected int _giveExp = 50;
    protected bool _isDead = false;

    string _skillType = "normal";

    protected void Start()
    {
        Initialized();
    }

    public virtual void Initialized()
    {
        if (_curHp < 0)
            _curHp = _maxHp;

        _isDead = false;
    }

    public virtual void Damage(int num, Vector3 targetPos, string skillType = "normal")
    {
        //if (skillType.Equals("overlap")) return;
        if (_skillType.Equals("overlap")) return;
        _skillType = skillType;
        _curHp -= num;
        Invoke("ChangeSkillType", 0.5f);
        if(_curHp <= 0)
        {
            Dead();
        }
        else
        {
            if (_skillType.Equals("overlap"))
            {
                HurtReaction(targetPos);
            }
        }
    }

    protected abstract void HurtReaction(Vector3 targetPos);

    /// <summary>
    /// 몬스터가 죽을 때마다 호출되는 함수 
    /// </summary>
    protected virtual void Dead()
    {
        if (transform.CompareTag(StringManager.enemyTag))
        {
            if(GetComponent<Status>().GetName() == "릴리")
            {
                // 마지막 보스 잡기 퀘스트 
                QuestManager.instance.CheckKillEnemyQuest(6);
            }

            GetComponent<Enemy>().GetPlayerStatus().IncreaseExp(_giveExp);

            // 현재 '몬스터 처치' 퀘스트를 진행 중이라면, 퀘스트 달성 조건 검사 
            QuestManager.instance.CheckKillEnemyQuest(GetComponent<Enemy>().Id);
        }

        _curHp = 0;
        _isDead = true;       
    }

    public string GetName() { return _name; }
    public int GetLevel() { return _level; }
    public int GetCurrentHp() { return _curHp; }
    public int SetCurrentHp(int hp) => _curHp = hp;
    public void IncreaseHp(int hp)
    {
        _curHp += hp;
        if (_curHp > _maxHp)
            _curHp = _maxHp;
    }
    public int GetMaxHp() { return _maxHp; }
    public int GetAtk() { return _atk; }
    public bool IsDead() { return _isDead; }
    public void ChangeSkillType()
    {
        _skillType = "normal";
    }
}
