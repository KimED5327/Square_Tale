using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint{
    public string priorMapName;
    public Transform tfSpawnPoint;
}

public class Map : MonoBehaviour
{
    [Header("Map Info")]
    [SerializeField] string _mapName = "MapName";
    
    [Header("Spawn Info")]
    [SerializeField] SpawnPoint[] _spawnPoints = null;

    // 넘어온 포탈에 따라 다른 스폰 포인트로 플레이어 이동.
    public void SearchSpawnPoint(Transform tfPlayer, string priorMapName)
    {
        for(int i = 0; i < _spawnPoints.Length; i++)
        {
            if(_spawnPoints[i].priorMapName == priorMapName)
            {
                tfPlayer.position = _spawnPoints[i].tfSpawnPoint.position;
                break;
            }
        }
    }

    public string GetMapName() { return _mapName; }
}
