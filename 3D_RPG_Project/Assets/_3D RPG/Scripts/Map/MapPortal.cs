using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPortal : MonoBehaviour
{
    MapManager _mapManager;
    [SerializeField] string _moveMapName = "Map_2";

    void Awake()
    {
        _mapManager = FindObjectOfType<MapManager>();
    }

    // 플레이어가 포탈에 닿으면 _moveMapName맵으로 이동 시도
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.instance.Save();
            _mapManager.ChangeMap(_moveMapName);
        }
    }
}
