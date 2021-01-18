using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public int id;
    public string name;
    public bool isBuff;
    public bool isTick;
    public float durationtime;
    public List<BuffOption> buffOption;
}
[System.Serializable]
public class BuffOption
{
    public BuffType buffType;
    public float applyBuffRate;
}

public enum BuffType
{
    HP,
    STR,
    INT,
    DEF,
    SPEED,
}


public class BuffData : MonoBehaviour
{
    public static BuffData instance;

    Dictionary<int, Buff> _buffDictionary = new Dictionary<int, Buff>();


    // 임시 DB. 시간 여유 있으면 JSON으로 대체

    void Awake()
    {
        instance = this;

        BuffOption option = new BuffOption { buffType = BuffType.STR, applyBuffRate = 0.1f };
        BuffOption option2 = new BuffOption { buffType = BuffType.INT, applyBuffRate = 0.1f };
        List<BuffOption> optionList = new List<BuffOption>();
        optionList.Add(option);
        optionList.Add(option2);

        Buff buff = new Buff { id = 1, name = "분노", isBuff = true, isTick = false, durationtime = 18f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.DEF, applyBuffRate = 0.1f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 2, name = "철벽", isBuff = true, isTick = false, durationtime = 18f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.SPEED, applyBuffRate = 0.1f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 3, name = "질풍", isBuff = true, isTick = false, durationtime = 18f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);


        option = new BuffOption { buffType = BuffType.HP, applyBuffRate = 0.025f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 4, name = "화상", isBuff = false, isTick = true, durationtime = 6f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.SPEED, applyBuffRate = 1f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 5, name = "속박", isBuff = false, isTick = false, durationtime = 3f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.SPEED, applyBuffRate = 0.5f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 6, name = "과중력", isBuff = false, isTick = false, durationtime = 7f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.HP, applyBuffRate = 0.015f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 7, name = "풀 베기", isBuff = false, isTick = true, durationtime = 12f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);

        option = new BuffOption { buffType = BuffType.DEF, applyBuffRate = 0.2f };
        optionList = new List<BuffOption>();
        optionList.Add(option);

        buff = new Buff { id = 8, name = "약점 포착", isBuff = false, isTick = false, durationtime = 16f, buffOption = optionList };
        _buffDictionary.Add(buff.id, buff);
    }


    public Buff GetBuff(int id)
    {
        if (_buffDictionary.ContainsKey(id))
        {
            return _buffDictionary[id];
        }
        else
        {
            Debug.LogError(id + " 는 등록되지 않은 버프 ID입니다");
            return null;
        }
    }
}
