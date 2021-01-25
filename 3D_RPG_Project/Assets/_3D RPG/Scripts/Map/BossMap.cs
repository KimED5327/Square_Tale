using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : MonoBehaviour
{
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public Boss boss;

    private void Update()
    {
        if(boss == null)
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<Boss>();

        if (boss.getIsDie())
        {
            particle1.gameObject.SetActive(true);
            particle2.gameObject.SetActive(true);
        }
       else
        {
            particle1.gameObject.SetActive(false);
            particle2.gameObject.SetActive(false);
        }
    }
}
