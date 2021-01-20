using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool _isDie = false;

    void Start()
    {
        instance = this;
    }
}
