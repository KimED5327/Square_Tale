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
    [SerializeField] int _levelUpInt = 3;
    [SerializeField] int _levelUpHp = 10;
    [SerializeField] int _levelUpDef = 1;

    //[SerializeField] int _levelUpSwordHp = 10;//임시 체력 증가량 (수정 필)
    //[SerializeField] int _levelUpMageHp = 8;//임시 체력 증가량 (수정 필)

    int _curExp = 0;
    bool _isLevelUp;

    public int GetStr() { return _str; }
    public int GetInt() { return _int; }
    public int GetDef() { return _def; }
    public int GetCurExp() { return _curExp; }
    public void SetStr(int str) { _str = str; }
    public void SetInt(int getInt) { _int = getInt; }
    public void SetDef(int def) { _def = def; }
    public void SetCurExp(int curExp) { _curExp = curExp; }
    public void AdjustInt(int num) { _int += num; }
    public void AdjustStr(int num) { _str += num; }
    public void AdjustDef(int num) { _def += num; }
    public void AdjustHp(int num) { _curHp += num; _maxHp += num; if (_curHp <= 0) _curHp = 1; }

    void Update()
    {
        LevelUp();
    }

    PlayerBuffManager _buffManager;

    public override void Initialized()
    {
        base.Initialized();
        if (_buffManager == null)
            _buffManager = FindObjectOfType<PlayerBuffManager>();
    }

    public void IncreaseExp(int num)
    {
        if (_level == 15) return;
        if (_level < _levelUpExps.Length)
        {
            _curExp += num;
            while (_curExp > _levelUpExps[_level - 1])
            {
                if (_levelUpExps[_level - 1] <= _curExp)
                {
                    _curExp -= _levelUpExps[_level - 1];
                    _maxHp += _levelUpHp;
                    _curHp = _maxHp;
                    //_swordMaxHp += _levelUpSwordHp;
                    //_swordCurHp = _swordMaxHp;
                    //_mageMaxHp += _levelUpMageHp;
                    //_mageCurHp = _mageMaxHp;
                    _str += _levelUpStr;
                    _int += _levelUpInt;
                    _def += _levelUpDef;
                    //_swordDef += _levelUpDef;
                    //_mageDef += _levelUpDef;
                    _level++;
                    _isLevelUp = true;
                    SaveManager.instance.Save();
                }
                if (_level == 15)
                {
                    _curExp = 0;
                    break;
                }
            }
        }
    }

    void LevelUp()
    {
        //if (Input.GetKeyDown("l") && _level < _levelUpExps.Length)
        if (Input.GetKeyDown("l"))
        {
            if (_level == 15) return;
            _curExp += 500;
            while (_curExp > _levelUpExps[_level - 1])
            {
                if (_levelUpExps[_level - 1] <= _curExp)
                {
                    _curExp -= _levelUpExps[_level - 1];
                    _maxHp += _levelUpHp;
                    _curHp = _maxHp;
                    //_swordMaxHp += _levelUpSwordHp;
                    //_swordCurHp = _swordMaxHp;
                    //_mageMaxHp += _levelUpMageHp;
                    //_mageCurHp = _mageMaxHp;
                    _str += _levelUpStr;
                    _int += _levelUpInt;
                    _def += _levelUpDef;
                    //_swordDef += _levelUpDef;
                    //_mageDef += _levelUpDef;
                    _level++;
                    _isLevelUp = true;
                    SoundManager.instance.PlayEffectSound("LevelUp");
                    SaveManager.instance.Save();
                }
                if (_level == 15)
                {
                    _curExp = 0;
                    break;
                }
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
        if (_level == 15) return 0;
        return (float)_curExp / _levelUpExps[_level - 1];
    }
    public float GetHpPercent()
    {
        return (float)_curHp / _maxHp;
    }

    public bool GetIsLevelUp() { return _isLevelUp; }
    public void SetIsLevelUp(bool isLevelUp) { _isLevelUp = isLevelUp; }

    public PlayerBuffManager GetBuffManager() { return _buffManager; }
}
