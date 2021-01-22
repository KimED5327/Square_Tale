using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    PlayerStatus _player;
    PlayerMove _playerTpye; // 플레이어의 현재 캐릭터?
    SkillManager _swordSkill;
    MageSkillManager _mageSkill;
    BlockManager _block;

    void Awake()
    {
        instance = this;

        _player = FindObjectOfType<PlayerStatus>();
        _playerTpye = FindObjectOfType<PlayerMove>();
        _swordSkill = FindObjectOfType<SkillManager>();
        _mageSkill = FindObjectOfType<MageSkillManager>();
        _block = FindObjectOfType<BlockManager>();
    }

    public void Save()
    {
        PlayerPrefs.SetString("name", _player.GetName());
        PlayerPrefs.SetInt("level", _player.GetLevel());

        //PlayerPrefs.SetInt("swordMaxHp", _player.GetSwordMaxHp());
        //PlayerPrefs.SetInt("swordCurHp", _player.GetSwordCurrentHp());
        //PlayerPrefs.SetInt("mageMaxHp", _player.GetMageMaxHp());
        //PlayerPrefs.SetInt("mageCurHp", _player.GetMageCurrentHp());
        PlayerPrefs.SetInt("maxHp", _player.GetMaxHp());
        PlayerPrefs.SetInt("curHp", _player.GetCurrentHp());

        PlayerPrefs.SetInt("str", _player.GetStr());
        PlayerPrefs.SetInt("int", _player.GetInt());

        //PlayerPrefs.SetInt("swordDef", _player.GetSwordDef());
        //PlayerPrefs.SetInt("mageDef", _player.GetMageDef());
        PlayerPrefs.SetInt("def", _player.GetDef());

        PlayerPrefs.SetInt("curExp", _player.GetCurExp());
        PlayerPrefs.SetString("swordType", _playerTpye.GetIsSword().ToString());
        PlayerPrefs.SetString("mageType", _playerTpye.GetIsMage().ToString());
        _swordSkill.Save();
        _mageSkill.Save();
        _block.Save();
        Debug.Log("save됨");
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            _player.SetName(PlayerPrefs.GetString("name"));
            _player.SetLevel(PlayerPrefs.GetInt("level"));

            //_player.SetSwordMaxHp(PlayerPrefs.GetInt("swordMaxHp"));
            //_player.SetSwordCurrentHp(PlayerPrefs.GetInt("swordCurHp"));
            //_player.SetMageMaxHp(PlayerPrefs.GetInt("mageMaxHp"));
            //_player.SetMageCurrentHp(PlayerPrefs.GetInt("mageCurHp"));
            _player.SetMaxHp(PlayerPrefs.GetInt("maxHp"));
            _player.SetCurrentHp(PlayerPrefs.GetInt("curHp"));

            _player.SetStr(PlayerPrefs.GetInt("str"));
            _player.SetInt(PlayerPrefs.GetInt("int"));

            //_player.SetSwordDef(PlayerPrefs.GetInt("swordDef"));
            //_player.SetMageDef(PlayerPrefs.GetInt("mageDef"));
            _player.SetDef(PlayerPrefs.GetInt("def"));

            _player.SetCurExp(PlayerPrefs.GetInt("curExp"));
            _playerTpye.SetIsSword(System.Convert.ToBoolean(PlayerPrefs.GetString("swordType")));
            _playerTpye.SetIsMage(System.Convert.ToBoolean(PlayerPrefs.GetString("mageType")));
            _swordSkill.Load();
            _mageSkill.Load();
            _block.Load();
            Debug.Log("load됨");
        }
    }
}
