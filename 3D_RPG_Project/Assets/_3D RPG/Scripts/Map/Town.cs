using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    PlayerStatus _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerStatus>();
        _player.SetCurrentHp(_player.GetMaxHp());
    }
}
