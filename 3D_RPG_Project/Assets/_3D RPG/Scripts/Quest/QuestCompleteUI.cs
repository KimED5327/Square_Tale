using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 완료 UI를 관리하는 클래스 
/// </summary>
public class QuestCompleteUI : MonoBehaviour
{
    [Header("퀘스트 완료 Panel")]
    [SerializeField] GameObject _questCompletedPanel = null;
    [SerializeField] Transform _rewardPanel = null;

    [Header("퀘스트 완료 UI")]
    [SerializeField] GameObject _rewardPrefab = null;
    [SerializeField] Text _txtTitle = null;
    [SerializeField] Sprite _imgGold = null;
    [SerializeField] Sprite _imgExp = null;
    [SerializeField] Sprite _imgKeyword = null;

    public void OpenQuestCompletePanel(Quest quest)
    {
        _questCompletedPanel.SetActive(true);
        _txtTitle.text = quest.GetTitle();
        ShowQuestRewards(quest);
    }

    public void CloseQuestCompletePanel()
    {
        _questCompletedPanel.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 슬롯으로부터 넘겨받은 리워드 데이터를 화면에 출력 
    /// </summary>
    /// <param name="rewards"></param>
    public void ShowQuestRewards(Quest quest)
    {
        // 기존의 모든 보상 삭제 
        DeleteAllRewards();

        AddGoldReward(quest);
        AddExpReward(quest);

        // 아이템 보상이 존재하는 경우 보상 추가 
        if (quest.GetItemID() != 0) AddItemReward(quest);

        // 블록 보상이 존재하는 경우 보상 추가 
        if (quest.GetBlockList().Count > 0) AddBlockReward(quest);

        // 키워드 보상이 존재하는 경우 보상 추가 
        if (quest.GetKeywordList().Count > 0) AddKeywordReward();
    }

    /// <summary>
    /// 리워드 패널의 자식 객체 모두 삭제 
    /// </summary>
    public void DeleteAllRewards()
    {
        foreach (Transform child in _rewardPanel)
        {
            Destroy(child.gameObject);
        }
    }

    // 골드 보상을 리워드 패널에 추가 
    void AddGoldReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgGold);
        reward.SetCount(quest.GetGold());
    }

    // 경험치 보상을 리워드 패널에 추가 
    void AddExpReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgExp);
        reward.SetCount(quest.GetExp());
    }

    // 아이템 보상을 리워드 패널에 추가 
    void AddItemReward(Quest quest)
    {
        //QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        //reward.SetImg(SpriteManager.instance.GetItemSprite(quest.GetItemID()));
        //reward.SetCount(1);

        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(SpriteManager.instance.GetItemSprite(8));
        reward.SetCount(1);

        QuestReward reward1 = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward1.SetImg(SpriteManager.instance.GetItemSprite(10));
        reward1.SetCount(1);
    }

    // 블록 보상을 리워드 패널에 추가 
    void AddBlockReward(Quest quest)
    {
        for (int i = 0; i < quest.GetBlockList().Count; i++)
        {
            QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

            reward.SetImg(SpriteManager.instance.GetBlockSprite(quest.GetBlock(i).GetBlockID()));
            reward.GetImg().GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 90f);
            reward.SetCount(quest.GetBlock(i).GetCount());
        }
    }

    // 키워드 보상을 리워드 패널에 추가 
    void AddKeywordReward()
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgKeyword);
        reward.TurnOffCount();
    }
}
