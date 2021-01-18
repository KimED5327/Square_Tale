using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    public PlayerBuffManager _pbm;


    private void Start()
    {
        _pbm = FindObjectOfType<PlayerBuffManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Face"))
        {
            _pbm.ApplyPlayerBuff(0);
        }
    }


}
