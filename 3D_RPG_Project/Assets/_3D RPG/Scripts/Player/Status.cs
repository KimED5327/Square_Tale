﻿using System.Collections;
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

    private void Start()
    {
        if (_curHp < 0)
            _curHp = _maxHp;
    }

    public void Damage(int num, Vector3 targetPos)
    {
       _curHp -= num;

       if(_curHp <= 0)
        {
            Dead();
        }
        else
        {
            HurtReaction(targetPos);
        }
    }

    protected abstract void HurtReaction(Vector3 targetPos);

    protected virtual void Dead()
    {
        if (transform.CompareTag(StringManager.enemyTag))
        {
            GetComponent<Enemy>().GetStatus().IncreaseExp(_giveExp);
        }
        _isDead = true;
    }


    public string GetName() { return _name; }
    public int GetLevel() { return _level; }
    public int GetCurrentHp() { return _curHp; }
    public int SetCurrentHp(int hp) => _curHp = hp;
    public int GetMaxHp() { return _maxHp; }
    public int GetAtk() { return _atk; }
    public bool IsDead() { return _isDead; }
}
