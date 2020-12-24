using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField] int _maxMp = 0;
    [SerializeField] int _curMp = 0;
    [SerializeField] int[] _levelUpExps = null;
    [SerializeField] int _curLevel = 0;
    [SerializeField] int _str = 0;
    [SerializeField] int _int = 0;

    int _curExp = 0;
    int _maxExp = 0;

    public int GetCurHp() { return _curHp; }
    public int GetCurMp() { return _curMp; }
    public int GetMaxHp() { return _maxHp; }
    public int GetMaxMp() { return _maxMp; }
    public int GetLevel() { return _curLevel; }
    public int GetStr() { return _str; }
    public int GetInt() { return _int; }
    public int GetDef() { return _def; }

    void Start()
    {
        _curExp = 0;
        _curLevel = 1;
        _maxHp = 150;
        _maxMp = 100;
        _curMp = _maxMp;
        _curHp = _maxHp;
        _str = 100;
        _int = 10;
        _def = 10;
        _atk = 10;
    }

    void Update()
    {
        LevelExp();
        LevelUp();
    }

    void LevelUp()
    {
        if (Input.GetKeyDown("l") && _curLevel < 15)
        {
            _curExp += 50;
            if (_maxExp <= _curExp)
            {
                _curExp -= _maxExp;
                _curLevel++;
                _maxHp += 10;
                _str += 2;
                _def++;
            }
        }
    }

    void LevelExp()
    {
        switch(_curLevel)
        {
            case 1:
                _maxExp = 100;
                break;
            case 2:
                _maxExp = 202;
                break;
            case 3:
                _maxExp = 306;
                break;
            case 4:
                _maxExp = 412;
                break;
            case 5:
                _maxExp = 520;
                break;
            case 6:
                _maxExp = 630;
                break;
            case 7:
                _maxExp = 742;
                break;
            case 8:
                _maxExp = 856;
                break;
            case 9:
                _maxExp = 972;
                break;
            case 10:
                _maxExp = 1090;
                break;
            case 11:
                _maxExp = 1200;
                break;
            case 12:
                _maxExp = 1332;
                break;
            case 13:
                _maxExp = 1456;
                break;
            case 14:
                _maxExp = 1582;
                break;
            case 15:
                _maxExp = 0;
                break;
        }
    }
}
