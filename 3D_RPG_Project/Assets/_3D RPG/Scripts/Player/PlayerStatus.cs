using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [Header("Player Status")]
    [SerializeField] int _str = 0;
    [SerializeField] int _int = 0;

    [Header("Level Up Info")]
    [SerializeField] int[] _levelUpExps = null;
    [SerializeField] int _levelUpStr = 2;
    [SerializeField] int _levelUpHp = 10;
    [SerializeField] int _levelUpDef = 1;

    int _curExp = 0;

    public int GetStr() { return _str; }
    public int GetInt() { return _int; }
    public int GetDef() { return _def; }

    void Start()
    {
        _curHp = _maxHp;
    }

    void Update()
    {
        LevelUp();
    }

    public void IncreaseExp(int num)
    {
        if (_level < _levelUpExps.Length)
        {
            _curExp += num;
            Debug.Log(num + "증가");
            if (_levelUpExps[_level] <= _curExp)
            {
                _curExp -= _levelUpExps[_level];
                _maxHp += _levelUpHp;
                _str += _levelUpStr;
                _def += _levelUpDef;
                _level++;
            }
        }
    }

    void LevelUp()
    {
        if (Input.GetKeyDown("l") && _level < _levelUpExps.Length)
        {
            _curExp += 50;
            if (_levelUpExps[_level] <= _curExp)
            {
                _curExp -= _levelUpExps[_level];
                _maxHp += _levelUpHp;
                _str += _levelUpStr;
                _def += _levelUpDef;
                _level++;
            }
        }
    }

    // 필요시 넉백 or 맞았을 때 반응 구현
    protected override void HurtReaction(Vector3 targetPos)
    {
        ;
    }

    public float GetExpPercent()
    {
        return (float)_curExp / _levelUpExps[_level];
    }
    public float GetHpPercent()
    {
        return (float)_curHp / _maxHp;
    }
}
