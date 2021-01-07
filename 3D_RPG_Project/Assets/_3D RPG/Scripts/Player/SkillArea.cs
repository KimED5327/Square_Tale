using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    PlayerStatus _status;

    bool isHit;

    float hitCount;

    // Start is called before the first frame update
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
    }

    void Update()
    {
        if (isHit)
        {
            hitCount += Time.deltaTime;
            if (hitCount > 5f)
            {
                isHit = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                if (!isHit)
                {
                    isHit = true;
                    //targetStatus.Damage(_status.GetAtk(), transform.position);
                    targetStatus.Damage(10, transform.position);
                }
            }
        }
    }
}
