using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCanDestroy : Block
{

    protected override void BlockEffect()
    {
        gameObject.SetActive(false);
    }
}
