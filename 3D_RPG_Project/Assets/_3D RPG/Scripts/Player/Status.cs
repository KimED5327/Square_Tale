using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected string _name = "이름";
    [SerializeField] protected int _maxHp = 100;
    [SerializeField] protected int _curHp = 100;
    [SerializeField] protected int _atk = 0;
    [SerializeField] protected int _def = 0;
    [SerializeField] protected int _level = 1;

    protected bool _isDead = false;

    public void Damage(int num)
    {
       _curHp -= num;

       if(_curHp <= 0)
        {
            Dead();
        }
    }


    protected virtual void Dead()
    {
        _isDead = true;
    }


    public string GetName() { return _name; }
    public int GetLevel() { return _level; }
    public int GetCurrentHp() { return _curHp; }
    public int GetMaxHp() { return _maxHp; }
    public bool IsDead() { return _isDead; }
}
