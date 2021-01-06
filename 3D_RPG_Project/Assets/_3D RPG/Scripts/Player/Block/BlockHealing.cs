using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealing : Block
{
    [SerializeField] float _healRatio = 0.2f;
    [SerializeField] float _radius = 3f;
    [SerializeField] LayerMask _layerMask = 0;

    protected override void BlockEffect()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        if (cols == null) return;
        Debug.Log(cols.Length);
        Status targetStatus = cols[0].transform.GetComponent<Status>();
        int heal = (int)(targetStatus.GetMaxHp() * _healRatio);
        targetStatus.IncreaseHp(heal);
    }
}
