using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    PlayerStatus _status;
    PlayerMove _player;

    // Start is called before the first frame update
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
        _player = FindObjectOfType<PlayerMove>();
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
                            targetStatus.Damage((int)(_status.GetInt() * (1.4f + (_status.GetLevel() / 10))), transform.position);
                            break;
                        case 4:
                            targetStatus.Damage((int)(_status.GetInt() * (1.9f + (_status.GetLevel() / 10))), transform.position);
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
                    //isHit = true;
                    switch (_player.getSkillNum())
                    {
                        case 2:
                               targetStatus.Damage((int)(_status.GetInt() * (0.2f + (_status.GetLevel() / 10))), transform.position, "overlap");
                            break;
                        case 3:
                               targetStatus.Damage((int)(_status.GetInt() * (0.7f + (_status.GetLevel() / 10))), transform.position, "overlap");
                            break;
                    }
                }
            }
        }
    }
}
