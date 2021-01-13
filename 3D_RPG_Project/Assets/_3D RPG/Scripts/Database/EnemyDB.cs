using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDB : MonoBehaviour
{
    public static EnemyDB instance;
    Dictionary<int, string> _enemyDB = new Dictionary<int, string>();

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyDB.Add(2, "이너비");
        _enemyDB.Add(3, "트리져");
        _enemyDB.Add(6, "릴리");
    }

    /// <summary>
    /// 에너미 ID에 맞는 이름을 string 값으로 반환 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetName(int key)
    {
        return _enemyDB[key];
    }

}
