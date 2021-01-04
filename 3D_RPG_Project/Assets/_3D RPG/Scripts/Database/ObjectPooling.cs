using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolInfo
{
    public string name;
    public GameObject goPrefab;
    public int count;
}

public class ObjectPooling : MonoBehaviour
{
    // 싱글턴
    public static ObjectPooling instance;

    // 풀 정보
    [SerializeField] PoolInfo[] _pools = null;
    
    // 풀 자료구조
    Queue<GameObject>[] _queues = null;
    
    // 임시 보관큐
    Queue<GameObject> _keepQueue;

    // 풀 딕셔너리
    Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    // 풀 초기화
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            _queues = new Queue<GameObject>[_pools.Length];

            for (int i = 0; i < _pools.Length; i++)
            {
                _queues[i] = MakePool(_pools[i].goPrefab, _pools[i].count);
                _poolDictionary.Add(_pools[i].name, _queues[i]);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 풀 구성하기
    Queue<GameObject> MakePool(GameObject obj, int count)
    {
        Queue<GameObject> queue = new Queue<GameObject>();

        for(int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(obj, Vector3.zero, Quaternion.identity);
            clone.transform.SetParent(this.transform);
            clone.SetActive(false);
            queue.Enqueue(clone);
        }

        return queue;
    }

    // 풀에서 오브젝트를 가져옴
    public GameObject GetObjectFromPool(string name, Vector3 pos)
    {
        // 딕셔너리에 키가 있다면-
        if (_poolDictionary.ContainsKey(name))
        {
            // 보관 큐에 캐싱 후...
            _keepQueue = _poolDictionary[name];
            
            // 풀에 하나라도 있으면-
            if (_keepQueue.Count > 0)
            {
                // 풀에서 오브젝트를 리턴시킨다.
                GameObject go = _poolDictionary[name].Dequeue();
                go.transform.position = pos;
                go.SetActive(true);
                return go;
            }
            // 아무것도 없다면-
            else
            {
                // 강제 생성 후 리턴시킨다.
                GameObject go = CreateObject(name);
                go.transform.position = pos;
                return go;
            }
        }

        // 키가 존재하지 않다면-
        else
        {
            Debug.LogError(name + "  이름의 Key는 오브젝트 풀에 등록되지 않았습니다.");
            return null;
        }
    }

    // 풀에 오브젝트가 없으면 강제로 생성시킴.
    GameObject CreateObject(string name)
    {
        for(int i = 0; i < _pools.Length; i++)
        {
            if(_pools[i].name == name)
            {
                GameObject go = Instantiate(_pools[i].goPrefab, Vector3.zero, Quaternion.identity);
                go.transform.SetParent(this.transform);
                return go; 
            }
        }

        Debug.LogError(name + "  이름의 Key는 오브젝트 풀에 등록되지 않았습니다.");
        return null;
        
    }

    // 오브젝트를 풀에 회수시킴.
    public void PushObjectToPool(string name, GameObject go) 
    {
        if (_poolDictionary.ContainsKey(name))
        {
            go.SetActive(false);

            _poolDictionary[name].Enqueue(go);
        }
        else
            Debug.LogError(name + "  이름의 Key는 오브젝트 풀에 등록되지 않았습니다.");
    }
}
