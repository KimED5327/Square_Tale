using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    public PlayerBuffManager _pbm;
    SphereCollider _sc;
    float _timer = 0f;
    float _maxTimer = 1.5f;
    Animator _ani;

    private void Start()
    {
        _ani = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
        _pbm = FindObjectOfType<PlayerBuffManager>();
        _sc = GetComponent<SphereCollider>();
    }


    
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other);

        if (other.transform.name != "Face")
            return;


       
        if(other.transform.name == "Face")
        {
            if(_sc.radius > 2f)
            {
                _pbm.ApplyPlayerBuff(2);
            
            }
        }

        Destroy(this);
    }





}
