using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    PlayerStatus _status;

    // Start is called before the first frame update
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                targetStatus.Damage(_status.GetAtk(), transform.position);
            }
        }
    }
}
