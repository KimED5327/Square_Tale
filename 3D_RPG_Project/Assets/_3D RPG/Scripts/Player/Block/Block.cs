using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : Status
{
    [SerializeField] protected string _destroyEffectName = "";    // 블록 파괴 연출 이펙트
    [SerializeField] protected string _destroySoundName = "Explosion";    // 블록 파괴 연출 이펙트

    [SerializeField] string _hitEffectName = "";        // 블록 피격 연출 이펙트 (플레이어)
    [SerializeField] float _destroyTime = 10f;
    [SerializeField] bool _autoDestoryBlock = false;
    [SerializeField] bool _isObjectPool = true;
    Transform _tfPlayer = null;
    protected Animator _anim = null;

    protected bool _canApply = true;

    static readonly string _aniPlayShake = "Shake";

    public void SetApplyCancel()
    {
        _canApply = false;
    }

    public override void Initialized()
    {
        if(_tfPlayer == null)
        {
            _tfPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            _anim = GetComponent<Animator>();
        }
        _canApply = true;
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

    public override void Damage(int num, Vector3 targetPos, string skillType = "normal", bool defenseIgnore = false)
    {
        _curHp -= 1;

        if (_curHp <= 0)
        {
            Dead();
        }
        else
        {
            HurtReaction(targetPos);
        }
    }


    public void ForceDestroy()
    {
        Dead();
    }

    protected override void Dead()
    {
        SoundManager.instance.PlayEffectSound(_destroySoundName);

        if (_hitEffectName != "")
            ObjectPooling.instance.GetObjectFromPool(_hitEffectName, _tfPlayer.position);
        if(_destroyEffectName != "")
            ObjectPooling.instance.GetObjectFromPool(_destroyEffectName, transform.position);
        
        _isDead = true;

        BlockEffect();

        if(_isObjectPool)
            ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
    }



    protected override void HurtReaction(Vector3 targetPos)
    {
        _anim.Play(_aniPlayShake);
    }

    protected abstract void BlockEffect();
}
