using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTwo : MonoBehaviour
{
    [SerializeField] string _spawnTwoName = "";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            MapManager.instance.SetSpawnPoint(_spawnTwoName);
        }
    }
}
