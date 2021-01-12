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
                string blockName = other.GetComponent<Block>().GetName();
                if (_blockName == blockName)
                {
                    ActiveTrigger();
                }
            }
        }
    }
}
