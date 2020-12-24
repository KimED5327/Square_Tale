using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected string _name;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _curHp;
    [SerializeField] protected int _atk;
    [SerializeField] protected int _def;

    protected bool _isDead = false;

    protected void Damage(int num)
    {
       
    }

    public bool IsDead() { return _isDead; }
    public string GetName() { return _name; }
}
