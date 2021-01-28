using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager : MonoBehaviour
{
    public static PlayerBuffManager instance;

    [SerializeField] BuffSlot[] _slots = null;

    PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Awake()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        instance = this;
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 버프 id값을 적용시킴
    /// </summary>
    /// <param name="id"></param>
    public void ApplyPlayerBuff(int id)
    { 
        // 버프 푸시
        bool isSuccess = TryPushBuff(id);

        if (!isSuccess)
            Debug.Log("버프가 가득찬 경우, 어떻게 처리할지 여부 필요");
    }


    // 버프 적용 시도
    bool TryPushBuff(int id)
    {
        // 같은 ID의 버프가 있다면 중첩 적용시킴
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].gameObject.activeSelf)
            {
                if (_slots[i].GetBuffID() == id)
                {
                    PushSlot(i, id, true);
                    return true;
                }
            }
        }

        // 빈 슬롯에 버프 적용
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].gameObject.activeSelf)
            {
                PushSlot(i, id, false);
                return true;
            }
        }
        return false;
    }

    // 슬롯에 버프 푸시
    void PushSlot(int index, int id, bool isDuplicate)
    {
        Buff buff = BuffData.instance.GetBuff(id);
        Color color = buff.isBuff ? Color.blue : Color.red;
        _slots[index].PushBuffSlot(id, buff.durationtime, color, buff.isTick);

        // 중첩이 아닐 경우에만 버프 능력치 반영
        if (!isDuplicate)
        {
            BuffApply(buff, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 적용중인 버프 카운트만큼 반복
        for(int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].gameObject.activeSelf)
            {
                // 버프 중이라면-
                if (!_slots[i].IsActive())
                {
                    // 버프 제거
                    _slots[i].DeActive();
                    BuffApply(BuffData.instance.GetBuff(_slots[i].GetBuffID()), false);
                }
                // 버프 지속중...
                else
                {
                    // Tick 호출이 있다면 (ex 매초마다 적용)
                    if (_slots[i].GetTickApply())
                    {
                        BuffApply(BuffData.instance.GetBuff(_slots[i].GetBuffID()), true);
                        _slots[i].ResetTick();
                    }
                }
            }
        }
    }


    void BuffApply(Buff buff, bool isApply)
    {

        for (int i = 0; i < buff.buffOption.Count; i++)
        {
            // 버프 적용 = 증가 , 버프 해제 = 감소
            float applyBuffRate = (isApply) ? buff.buffOption[i].applyBuffRate
                                            : -buff.buffOption[i].applyBuffRate;

            // 디버프는 반대로
            if (!buff.isBuff)
                applyBuffRate *= -1f;


            // 버프 유형에 따라 적용
            switch (buff.buffOption[i].buffType)
            {
                case BuffType.HP:
                    // 디버프는 틱데미지 적용
                    if(!buff.isBuff)
                        _playerStatus.Damage((int)(_playerStatus.GetMaxHp() * Mathf.Abs(applyBuffRate)), Vector3.zero, "normal" ,true);
                    break;
                case BuffType.INT:
                    _playerStatus.AdjustInt((int)(_playerStatus.GetInt() * applyBuffRate));
                    break;
                case BuffType.STR:
                    _playerStatus.AdjustStr((int)(_playerStatus.GetStr() * applyBuffRate));
                    break;
                case BuffType.DEF:
                    _playerStatus.AdjustDef((int)(_playerStatus.GetDef() * applyBuffRate));
                    break;
                case BuffType.SPEED:
                    _playerStatus.GetComponent<PlayerMove>().applySpeed += applyBuffRate;
                    break;

            }
        }
    }

}
