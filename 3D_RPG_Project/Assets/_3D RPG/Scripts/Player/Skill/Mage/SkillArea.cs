using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    PlayerStatus _status;
    PlayerMove _player;

    bool isHit;

    float hitCount;

    // Start is called before the first frame update
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
        _player = FindObjectOfType<PlayerMove>();
    }

    void Update()
    {
        if (isHit)
        {
            hitCount += Time.deltaTime;
            if (hitCount > 1f)
            {
                hitCount = 0;
                isHit = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player.getSkillNum() == 1 || _player.getSkillNum() == 4)
        {
            if (other.CompareTag("Enemy"))
            {
                Status targetStatus = other.GetComponent<Status>();
                if (!targetStatus.IsDead())
                {
                    switch (_player.getSkillNum())
                    {
                        case 1:
                            targetStatus.Damage((int)(_status.GetInt() * 1f), transform.position);
                            break;
                        case 4:
                            targetStatus.Damage((int)(_status.GetInt() * 2f), transform.position);
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_player.getSkillNum() == 2 || _player.getSkillNum() == 3)
        {
            if (other.CompareTag("Enemy"))
            {
                Status targetStatus = other.GetComponent<Status>();
                if (!targetStatus.IsDead())
                {
                    if (!isHit)
                    {
                        isHit = true;
                        switch (_player.getSkillNum())
                        {
                            case 2:
                                targetStatus.Damage((int)(_status.GetInt() * 0.6f), transform.position);
                                break;
                            case 3:
                                targetStatus.Damage((int)(_status.GetInt() * 0.8f), transform.position);
                                break;
                        }
                    }
                }
            }
        }
    }
}
