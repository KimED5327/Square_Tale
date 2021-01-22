using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDamageApply : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            PlayerStatus st =  other.GetComponent<PlayerStatus>();
            st.Damage((int)(st.GetMaxHp() * 0.3f), Vector3.zero);

        }
    }
}
