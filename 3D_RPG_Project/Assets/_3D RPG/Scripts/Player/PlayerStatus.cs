using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField] int _maxMp;
    [SerializeField] int _curMp;
    [SerializeField] int[] _levelUpExps;
    [SerializeField] int _culLevel;
    [SerializeField] int _str;
    [SerializeField] int _int;

    int _curExp;
}
