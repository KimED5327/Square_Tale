using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager : MonoBehaviour
{
    [SerializeField] BuffSlot[] _slots = null;

    PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Awake()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();

        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].gameObject.SetActive(false);
        }
    }
    
    public void ApplyPlayerBuff(int id)
    {
        // 버프 중첩 시도
        if (!PushSameBuffSlot(id))
        {
            // 빈 슬롯에 버프 푸시
            if (!PushAnyEmptySlot(id))
            {
                // 빈 슬롯이 없는 경우
                Debug.Log("버프가 가득찬 경우, 어떻게 처리할지 여부 필요");
            }
        }
    }

    bool PushSameBuffSlot(int id)
    {
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
        return false;
    }

    bool PushAnyEmptySlot(int id)
    {
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

    void PushSlot(int index, int id, bool isDuplicate)
    {
        Buff buff = BuffData.instance.GetBuff(id);
        Color color = buff.isBuff ? Color.blue : Color.red;
        _slots[index].PushBuffSlot(id, buff.durationtime, color, buff.isTick);
        if (!isDuplicate)
            BuffApply(buff, true);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            // 버프가 끝났고 활성화된 슬롯이라면-
            if (!_slots[i].IsActive() && _slots[i].gameObject.activeSelf)
            {
                // 버프 제거 
                _slots[i].DeActive();
                BuffApply(BuffData.instance.GetBuff(_slots[i].GetBuffID()), false);
            }

            // 버프 중에 Tick 호출이 있다면 (ex 매초마다 적용)
            if (_slots[i].IsActive() && _slots[i].GetTickApply())
            {
                BuffApply(BuffData.instance.GetBuff(_slots[i].GetBuffID()), true);
                _slots[i].ResetTick();
            }
        }

        // 테스트
        if (Input.GetKeyDown(KeyCode.B))
        {
            ApplyPlayerBuff(1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ApplyPlayerBuff(2);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ApplyPlayerBuff(3);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            ApplyPlayerBuff(4);
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ApplyPlayerBuff(5);
        }
    }


    void BuffApply(Buff buff, bool isApply)
    {
        for(int i = 0; i < buff.buffOption.Count; i++)
        {
            // 버프 적용 시에는 더해주고, 취소시에는 빼준다.
            float applyBuffRate = (isApply) ? buff.buffOption[i].applyBuffRate
                                            : -buff.buffOption[i].applyBuffRate;

            switch (buff.buffOption[i].buffType)
            {
                case BuffType.HP:
                    // 디버프인 경우 틱 데미지 적용
                    if(!buff.isBuff)
                        _playerStatus.Damage((int)(_playerStatus.GetMaxHp() * applyBuffRate), Vector3.zero);
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
                    // 디버프인 경우, 다시 반대로.
                    if (!buff.isBuff)
                        _playerStatus.GetComponent<PlayerMove>().applySpeed -= applyBuffRate;
                    else
                        _playerStatus.GetComponent<PlayerMove>().applySpeed += applyBuffRate;
                    break;

            }
        }


    }

}
