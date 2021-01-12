using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCheck : MonoBehaviour
{

    [SerializeField] BlockCrystal _crystalBlock = null;


    public void AcquireCrystal()
    {
        _crystalBlock.SetAcquireCrystal();
        gameObject.SetActive(false);
    }


}
