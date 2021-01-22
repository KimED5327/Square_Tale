using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlockPlate : Trigger
{
    [SerializeField] string _blockName = "폭발 블록";

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
                }
            }
        }
    }
}
