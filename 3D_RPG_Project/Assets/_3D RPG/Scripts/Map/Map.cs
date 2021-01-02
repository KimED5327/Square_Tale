using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnPoint{
    public string priorMapName;
    public Transform tfSpawnPoint;
}

[System.Serializable]
public class MonsterSpawnPoint
{
    public string monsterName;
    public Transform tfSpawnLocation;
    public int count;
    public float spawnRange;
}


public class SpawnMonster
{
    public EnemyStatus enemyStatus;
    public int spawnPointIndex;
}

public class Map : MonoBehaviour
{
    // 현재 맵 정보
    [Header("Map Info")]
    [SerializeField] string _mapName = "MapName";

    // 플레이어 맵 이동 스폰 위치
    [Header("Spawn Info")]
    [SerializeField] SpawnPoint[] _spawnPoints = null;

    // 몬스터 스폰 정보 관리
    [Header("Monster Spawn Info")]
    [SerializeField] MonsterSpawnPoint[] _monsterSpawnPoint = null;
    [SerializeField] float _respawnCheckTime = 1f;
    WaitForSeconds waitTime;

    // 스폰된 몬스터 관리.
    List<SpawnMonster> _spawnEnemyList = new List<SpawnMonster>();

    void Awake()
    {
        waitTime = new WaitForSeconds(_respawnCheckTime);
    }

    // 맵 활성화되면 몬스터들 스폰.
    void OnEnable()
    {
        // 스폰 정보가 있으면 실행
        if(_monsterSpawnPoint != null && _monsterSpawnPoint.Length > 0)
        {
            SpawnMonster();
            StartCoroutine(NeedRespawnMonster());
        }
    }

    // 맵 비활성화되면 스폰된 몬스터 전부 클리어
    void OnDisable()
    {
        StopAllCoroutines();

        for (int i = 0; i < _spawnEnemyList.Count; i++)
        {
            if (_spawnEnemyList[i].enemyStatus == null) break;

            GameObject goEnemy = _spawnEnemyList[i].enemyStatus.gameObject;
            string monsterName = _spawnEnemyList[i].enemyStatus.GetName();

            ObjectPooling.instance.PushObjectToPool(monsterName, goEnemy);
        }
        _spawnEnemyList.Clear();

    }

    // 몬스터 스폰 후 몬스터 스폰 관리 리스트에 넣어줌.
    void SpawnMonster()
    {
        for (int i = 0; i < _monsterSpawnPoint.Length; i++)
        {
            for (int k = 0; k < _monsterSpawnPoint[i].count; k++)
            {
                // 몬스터 생성 or 풀링
                GameObject enemy = MonsterSetting(_monsterSpawnPoint[i]);
                SpawnMonster spawnMonster = new SpawnMonster()
                {
                    enemyStatus = enemy.GetComponent<EnemyStatus>(),
                    spawnPointIndex = i,
                };

                _spawnEnemyList.Add(spawnMonster);
            }
        }
    }

    // 몬스터 꺼내오고 위치 세팅
    GameObject MonsterSetting(MonsterSpawnPoint spawnInfo)
    {
        string monsterName = spawnInfo.monsterName;
        float range = spawnInfo.spawnRange;
        Vector3 pos = spawnInfo.tfSpawnLocation.position;
        Vector3 rot = new Vector3(0f, Random.Range(0f, 180f), 0f);

        pos.x += Random.Range(-range, range);
        pos.z += Random.Range(-range, range);

        GameObject enemy = ObjectPooling.instance.GetObjectFromPool(monsterName, pos);
        enemy.transform.eulerAngles = rot;

        return enemy;
    }


    // 매 n초마다 죽어서 '비활성화된' 에너미 체크 후, 새로 스폰시킴.
    IEnumerator NeedRespawnMonster()
    {
        while (true)
        {
            yield return waitTime;
            for(int i = 0; i < _spawnEnemyList.Count; i++)
            {
                if (!_spawnEnemyList[i].enemyStatus.gameObject.activeSelf)
                {
                    MonsterSpawnPoint spawnInfo = _monsterSpawnPoint[_spawnEnemyList[i].spawnPointIndex];
                    GameObject enemy = MonsterSetting(spawnInfo);
                    _spawnEnemyList[i].enemyStatus = enemy.GetComponent<EnemyStatus>();
                }

            }
        }
    }



    // 넘어온 포탈에 따라 다른 스폰 포인트로 플레이어 이동.
    public void SearchSpawnPoint(Transform tfPlayer, string priorMapName)
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (_spawnPoints[i].priorMapName == priorMapName)
            {
                tfPlayer.position = _spawnPoints[i].tfSpawnPoint.position;
                tfPlayer.rotation = _spawnPoints[i].tfSpawnPoint.rotation;
                tfPlayer.GetComponent<CameraController>().SetCamRot(_spawnPoints[i].tfSpawnPoint.eulerAngles.y);
                break;
            }
        }
    }

    // 현재 맵 이름 가져오기
    public string GetMapName() { return _mapName; }
}
