using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundInfo
{
    public string name;
    public AudioClip audio;
    public float pithc;
}

public class SoundManager : MonoBehaviour
{
    //싱글턴
    public static SoundManager instance;

    //풀 정보
    [SerializeField] SoundInfo[] _sound = null;

    //풀 자료구조
    Queue<AudioClip>[] _queues = null;

    //풀 딕셔너리
    Queue<AudioClip> _keepQueue;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
            _queues = new Queue<AudioClip>[_sound.Length];

            for(int i = 0; i < _sound.Length; ++i)
            {
                //_queues[i] = MakePool
            }
        }
    }

    Queue<AudioClip> MakePool(AudioClip audio, float pithc)
    {
        Queue<AudioClip> queue = new Queue<AudioClip>();

        

        return queue;
    }


}
