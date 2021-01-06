using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    Transform player;
    EnemyStatus states;

    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       states = GetComponentInParent<EnemyStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.GetComponent<Status>().Damage(states.GetAtk(), transform.position);
        }
    }
}
