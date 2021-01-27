using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    SphereCollider _sc;
    [SerializeField] float _maxTimer = 1.5f;
    Transform _target;
    private void Start()
    {
        _sc = GetComponent<SphereCollider>();
        Invoke(nameof(ActiveDebuff), _maxTimer);
    }

    public void TargetLink(Transform target)
    {
        _target = target;
    }

    public void ActiveDebuff()
    {

        if (_target.CompareTag(StringManager.playerTag))
        {
            Vector3 front = _target.forward;
            Vector3 dir = (transform.position - _target.position).normalized;
            float viewAngle = Vector3.Dot(front, dir);
            Debug.Log(viewAngle * Mathf.Rad2Deg);

            if (viewAngle >= Mathf.Cos(90f * Mathf.Deg2Rad))
            {
                PlayerBuffManager.instance.ApplyPlayerBuff(6);
                PlayerBuffManager.instance.ApplyPlayerBuff(7);
                PlayerBuffManager.instance.ApplyPlayerBuff(8);
            }

        }
        Invoke(nameof(DestroyTime), 2f);
    }

    void DestroyTime()
    {
        Destroy(this);
    }

}
