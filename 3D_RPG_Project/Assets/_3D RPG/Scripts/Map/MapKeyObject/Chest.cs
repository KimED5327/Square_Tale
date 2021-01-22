using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType
{
    KEYWORD,
    ALLBLOCK,
    BLOCK,
    TRAP,
    ITEM,
    GOLD,
    All,
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

    GameObject _goChild;

    bool _isOpen = false;

    void Awake()
    {
        _goChild = transform.GetChild(0).gameObject;

        if (!PlayerPrefs.HasKey(StringManager.GetTresureKey(_treasureID)))
            PlayerPrefs.SetInt(StringManager.GetTresureKey(_treasureID), NOT_ACQUIRE_REWARD);
        else
        {
            int rewardCheck = PlayerPrefs.GetInt(StringManager.GetTresureKey(_treasureID));
            if (rewardCheck == ACQUIRE_REWARD)
                gameObject.SetActive(false);
        }
    }

    public Reward GetReward() {

        if (!_isOpen)
        {
            _isOpen = true;
            _goChild.SetActive(false);
            SoundManager.instance.PlayEffectSound("ChestOpen");
            return _reward;
        }
        else
        {
            return null;
        }

    }

    public void RemoveChest()
    {
        PlayerPrefs.SetInt(StringManager.GetTresureKey(_treasureID), ACQUIRE_REWARD);
        StartCoroutine(WaitDisappear());
    }

    IEnumerator WaitDisappear()
    {
        // 연출
        if(_reward.rewardType == RewardType.GOLD)
        {
            GameObject go = ObjectPooling.instance.GetObjectFromPool("보물 이펙트", transform.position);
            go.GetComponent<TreasureEffect>().SetColor(false);
        }
        else if(_reward.rewardType != RewardType.TRAP)
        {
            GameObject go = ObjectPooling.instance.GetObjectFromPool("보물 이펙트", transform.position);
            go.GetComponent<TreasureEffect>().SetColor(true);
        }

        if (_reward.soundName != "")
            SoundManager.instance.PlayEffectSound(_reward.soundName);
        if (_reward.effectName != "")
            ObjectPooling.instance.GetObjectFromPool(_reward.effectName, transform.position);

        yield return new WaitForSeconds(3f);

        ObjectPooling.instance.GetObjectFromPool("발판 블록 파괴 이펙트", transform.position + Vector3.up * 0.3f);


        gameObject.SetActive(false);
    }
}
