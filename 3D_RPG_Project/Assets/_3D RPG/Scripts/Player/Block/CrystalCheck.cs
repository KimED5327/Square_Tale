using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCheck : MonoBehaviour, IMustNeedItem
{
    [SerializeField] BlockCrystal _crystalBlock = null;

    public void AcquireItem()
    {
        _crystalBlock.SetAcquireCrystal();
        gameObject.SetActive(false);
    }

}
