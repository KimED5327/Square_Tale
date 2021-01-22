using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : MonoBehaviour
{

    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public Boss _boss;



    void Update()
    {
        if(_boss == null) _boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Boss>();


        if (_boss.getIsDie())
        {
            particle1.gameObject.SetActive(true);
            particle2.gameObject.SetActive(true);
        }
    }
}
