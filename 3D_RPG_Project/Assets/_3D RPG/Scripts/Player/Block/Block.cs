using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : Status
{
    [SerializeField] string _destroyEffectName = "";    // 블록 파괴 연출 이펙트
    [SerializeField] string _hitEffectName = "";        // 블록 피격 연출 이펙트 (플레이어)
    [SerializeField] float _destroyTime = 10f;
    [SerializeField] bool _autoDestoryBlock = false;
    Transform _tfPlayer = null;
    Animator _anim = null;

    static readonly string _aniPlayShake = "Shake";

    public override void Initialized()
    {
        if(_tfPlayer == null)
        {
            _tfPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            _anim = GetComponent<Animator>();
        }

        _curHp = _maxHp;
        _isDead = false;

        if(_autoDestoryBlock)
            Invoke("AutoDestory", _destroyTime);
    }

    void AutoDestory()
    {
        if (gameObject.activeSelf)
        {
            Dead();
        }
    }

    //public override void Damage(int num, Vector3 targetPos)
    //{
    //    _curHp -= 1;

    //    if (_curHp <= 0)
    //    {
    //        Dead();
    //    }
    //    else
    //    {
    //        HurtReaction(targetPos);
    //    }
    //}


    protected override void Dead()
    {
        if (_hitEffectName != "")
        {
            ObjectPooling.instance.GetObjectFromPool(_hitEffectName, _tfPlayer.position);
        }
        ObjectPooling.instance.GetObjectFromPool(_destroyEffectName, transform.position);
        _isDead = true;

        BlockEffect();

        ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
    }



    protected override void HurtReaction(Vector3 targetPos)
    {
        _anim.Play(_aniPlayShake);
    }

    protected abstract void BlockEffect();
}
