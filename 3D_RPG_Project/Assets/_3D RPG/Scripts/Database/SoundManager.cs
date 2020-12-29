using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundInfo
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    //싱글턴
    public static SoundManager instance;

    //풀 정보
    [SerializeField] SoundInfo[] effectSounds;
    [SerializeField] AudioSource[] effectPlayer;

    [SerializeField] SoundInfo[] bgmSound;
    [SerializeField] AudioSource bgmPlayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayBGM(string p_name)
    {
        for(int i = 0;i < bgmSound.Length; i++)
        {
            if(p_name == bgmSound[i].name)
            {
                bgmPlayer.clip = bgmSound[i].clip;
                bgmPlayer.Play();
                return;
            }
        }

        Debug.LogError(p_name + "에 해당되는 BGM이 없습니다");

    }

    void StopBGM()
    {
        bgmPlayer.Stop();
    }
    void PauseBGM()
    {
        bgmPlayer.Pause();
    }
    void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }
    void PlayEffectSound(string p_name)
    {
        for(int i = 0; i < effectSounds.Length;i++)
        {
            if(p_name == effectSounds[i].name)
            {
                for(int j = 0; j < effectPlayer.Length; j++)
                {
                    if(!effectPlayer[j].isPlaying)
                    {
                        effectPlayer[j].clip = effectSounds[i].clip;
                        effectPlayer[j].Play();
                        return;
                    }
                    
                }
                Debug.LogError("모든 효과음 플레이어가 사용중입니다.");
                return;
            }
            Debug.LogError(p_name + "에 해당하는 효과음 사운드가 없습니다");
        }
    }

    void StopAllEffectSound()
    {
        for(int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop();
        }
    }

    public void PlaySound(string p_name, int p_Type)
    {
        if (p_Type == 0) PlayBGM(p_name);
        else if (p_Type == 1) PlayEffectSound(p_name);
    }
}
