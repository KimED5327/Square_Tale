using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Map[] _maps = null;
    [SerializeField] Map _currentMap = null;

    // 맵 전환
    // 이후 로딩씬 -> 게임씬으로 전환할 예정
    public void ChangeMap(Transform tfPlayer, string moveMapName)
    {
        for(int i = 0; i < _maps.Length; i++)
        {
            string mapName = _maps[i].GetMapName();
            if (mapName == moveMapName)
            {
                string currentMapName = _currentMap.GetMapName();

                // 기존 맵 비활성화
                _currentMap.gameObject.SetActive(false);
                
                // 교체
                _currentMap = _maps[i];
                
                // 교체된 맵 활성화 후, 플레이어 스폰 위치 조정.
                _currentMap.gameObject.SetActive(true);
                _currentMap.SearchSpawnPoint(tfPlayer, currentMapName);
                break;
            }
        }
    }
}
