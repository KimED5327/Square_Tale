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

    public int GetMaxHp() { return _maxHp; }
    public int GetMaxMp() { return _maxMp; }
    public int GetLevel() { return _curLevel; }
    public int GetStr() { return _str; }
    public int GetInt() { return _int; }
    public int GetDef() { return _def; }
}
