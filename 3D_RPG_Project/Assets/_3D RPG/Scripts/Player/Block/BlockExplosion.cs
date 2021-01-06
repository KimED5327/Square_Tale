using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockExplosion : Block
{
    [SerializeField] float _damageRatio = 0.2f;
    [SerializeField] float _radius = 3f;
    [SerializeField] LayerMask _layerMask = 0;

    protected override void BlockEffect()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        if (cols == null) return;

        for(int i = 0; i < cols.Length; i++)
        {
            Status targetStatus = cols[i].transform.GetComponent<Status>();
            int damage = (int)(targetStatus.GetMaxHp() * _damageRatio);
            targetStatus.Damage(damage, transform.position);
        }
    }
}
