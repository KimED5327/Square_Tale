using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlockPlate : Trigger
{
    [SerializeField] string _blockName = "폭발 블록";

    [SerializeField] float _blockDestroyTime = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.blockTag))
        {
            if (!_isActivated)
            {
                Block block = other.GetComponent<Block>();
                string blockName = block.GetName();
                if (_blockName == blockName)
                {
                    block.SetApplyCancel();
                    ActiveTrigger();
                    StartCoroutine(DestoryBlock(block));
                }
            }
        }
    }

    IEnumerator DestoryBlock(Block block)
    {
        yield return new WaitForSeconds(_blockDestroyTime);

        block.ForceDestroy();
    }
}
