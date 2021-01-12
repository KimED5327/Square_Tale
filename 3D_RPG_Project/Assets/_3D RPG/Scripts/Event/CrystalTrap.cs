using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalTrap : MonoBehaviour
{
    [SerializeField] Transform[] _tfGroups = null;
    int _groupIndex = 0;

    [SerializeField] float _height = 0.5f;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _waitTime = 1.5f;
    [SerializeField] float _delayTime = 0.2f;
    [SerializeField] float _backTime = 0.5f;
    float _curWaitTime = 0f;
    float _curDelayTime = 0f;
    float _curBackTime = 0f;

    float _damageCoolTime = 0.25f;
    float _curDamageCoolTime = 0f;
    bool _canDamaged = true;

    Vector3[] _destinationPos;
    Vector3[] _originPos;

    private void Start()
    {
        _originPos = new Vector3[_tfGroups.Length];
        _destinationPos = new Vector3[_tfGroups.Length];
        for (int i = 0; i < _originPos.Length; i++)
        {
            _originPos[i] = _tfGroups[i].position;
            _destinationPos[i] = _originPos[i] + Vector3.up * _height;
        }
    }

    private void Update()
    {

        DamageCoolTimeCalc();

        CrystalMove();
    }

    private void CrystalMove()
    {
        // 대기 타임이 끝나면 공격
        if (_curDelayTime >= _delayTime)
        {
            // 공격 타임이 끝나면 복귀
            if (_curWaitTime >= _waitTime)
            {
                // 복귀가 끝나면 초기화
                if (_curBackTime >= _backTime)
                {
                    _curWaitTime = 0f;
                    _curDelayTime = 0f;
                    _curBackTime = 0f;
                    if (++_groupIndex >= _tfGroups.Length)
                    {
                        _groupIndex = 0;
                    }
                }
                // 복귀중
                else
                {
                    _curBackTime += Time.deltaTime;
                    MoveGroup(_groupIndex, _originPos[_groupIndex]);
                }
            }
            // 공격중
            else
            {
                MoveGroup(_groupIndex, _destinationPos[_groupIndex]);
                _curWaitTime += Time.deltaTime;
            }
        }
        // 대기중
        else
        {
            _curDelayTime += Time.deltaTime;
        }
    }

    void DamageCoolTimeCalc()
    {
        if (!_canDamaged)
        {
            _curDamageCoolTime += Time.deltaTime;
            if (_curDamageCoolTime >= _damageCoolTime)
            {
                _curDamageCoolTime = 0f;
                _canDamaged = true;
            }
        }
    }

    void MoveGroup(int index, Vector3 dest)
    {
        Vector3 pos = _tfGroups[index].position;
        pos = Vector3.Lerp(pos, dest, _speed * Time.deltaTime);
        _tfGroups[index].position = pos;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_canDamaged)
        {
            if (other.CompareTag(StringManager.playerTag))
            {
                Status status = GetComponent<Status>();
                int damage = (int)(status.GetMaxHp() * 0.3f);
                status.Damage(damage, other.transform.position);
                _canDamaged = false;
            }
        }

    }
}
