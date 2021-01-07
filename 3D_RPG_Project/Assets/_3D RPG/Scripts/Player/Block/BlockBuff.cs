using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuff : Block
{
    [SerializeField] int[] _buffID = null;
    [SerializeField] float _radius = 3f;
    [SerializeField] LayerMask _layerMask = 0;

    protected override void BlockEffect()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        if (cols.Length == 0) return;

        int randomIndex = Random.Range(0, _buffID.Length);
        int randomBuffID = _buffID[randomIndex];

        PlayerBuffManager buffManager = cols[0].transform.GetComponent<PlayerStatus>().GetBuffManager();
        buffManager.ApplyPlayerBuff(randomBuffID);
    }
}
