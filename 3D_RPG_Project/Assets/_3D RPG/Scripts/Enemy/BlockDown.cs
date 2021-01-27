using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDown : MonoBehaviour
{

    [SerializeField] GameObject[] _BlockGroup;
    static bool[] _GroupDown = new bool[10];
    private float _timer;

    void Start()
    {
        _timer = 0f;

        for(int i = 0; i < 10; i++)
        {
            _GroupDown[i] = false;
        }
    }


    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > 30)
        {
            for(int i = 0; i < _BlockGroup.Length; i++)
            {
                if (_GroupDown[i] == false)
                {
                  
                    _GroupDown[i] = true;
                    _timer = 0;
                    break;
                }

            }
        }

        for(int i = 0; i < 10; i++)
        {
            if(_GroupDown[i] == false) return;
            if(_GroupDown[i] == true)
            {
                _BlockGroup[i].transform.position = new Vector3(_BlockGroup[i].transform.position.x, _BlockGroup[i].transform.position.y - 0.1f, _BlockGroup[i].transform.position.z);

                if(_BlockGroup[i].transform.position.y < -10)
                {
                   _BlockGroup[i].gameObject.SetActive(false);
                }
            }

        }
    }
}
