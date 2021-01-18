using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawnEnemy : MonoBehaviour
{
    [SerializeField] string _monsterName = "블루벨";
    GameObject _enemy;
    TriggerMonster _trigger;

    [SerializeField] bool _isEvent = true;
    [SerializeField] bool _isBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform tfPlayer = FindObjectOfType<PlayerMove>().transform;

        _enemy = ObjectPooling.instance.GetObjectFromPool(_monsterName, transform.position);
        _enemy.transform.rotation = transform.rotation;

        if(!_isBoss)
        {
            _enemy.GetComponent<Enemy>().LinkPlayer(tfPlayer);
        }
            

        if (_isEvent)
        {
            _trigger = GetComponent<TriggerMonster>();
            _trigger.SetTargetLink(_enemy.GetComponent<EnemyStatus>());
        }
    }

    private void OnDisable()
    {
        ObjectPooling.instance.PushObjectToPool(_monsterName, _enemy);
    }
}
