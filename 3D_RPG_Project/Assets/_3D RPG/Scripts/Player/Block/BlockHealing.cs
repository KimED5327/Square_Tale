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
        Debug.Log("호출됨");

        Collider[] cols = Physics.OverlapSphere(transform.position, _radius, _layerMask);


        if (cols.Length == 0) return;

        Debug.Log(cols[0].transform.name);

        for (int i = 0; i < cols.Length; i++)
        {

            Status targetStatus = cols[0].transform.GetComponent<Status>();

            if (targetStatus == null) continue;

            int heal = (int)(targetStatus.GetMaxHp() * _healRatio);
            targetStatus.IncreaseHp(heal);
        }
    }
}
