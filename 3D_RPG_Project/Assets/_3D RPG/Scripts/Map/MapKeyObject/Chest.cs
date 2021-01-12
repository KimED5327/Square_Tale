using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType
{
    KEYWORD,
    ALLBLOCK,
    BLOCK,
    TRAP,
}

[System.Serializable]
public class Reward
{
    public RewardType rewardType;
    public int id;
    public int count;
    public string soundName;
    public string effectName;
}

public class Chest : MonoBehaviour
{
    const int ACQUIRE_REWARD = 1, NOT_ACQUIRE_REWARD = 0;

    [SerializeField] int _treasureID = 0;
    [SerializeField] Reward _reward = null;

    void Awake()
    {
        if (!PlayerPrefs.HasKey(StringManager.GetTresureKey(_treasureID)))
            PlayerPrefs.SetInt(StringManager.GetTresureKey(_treasureID), NOT_ACQUIRE_REWARD);
        else
        {
            int rewardCheck = PlayerPrefs.GetInt(StringManager.GetTresureKey(_treasureID));
            if (rewardCheck == ACQUIRE_REWARD)
                gameObject.SetActive(false);
        }
    }

    public Reward GetReward() { return _reward; }

    public void RemoveChest()
    {
        if(_reward.soundName != "")
            SoundManager.instance.PlayEffectSound(_reward.soundName);
        if(_reward.effectName != "")
            ObjectPooling.instance.GetObjectFromPool(_reward.effectName, transform.position);

        PlayerPrefs.SetInt(StringManager.GetTresureKey(_treasureID), ACQUIRE_REWARD);
        gameObject.SetActive(false);
    }
}
