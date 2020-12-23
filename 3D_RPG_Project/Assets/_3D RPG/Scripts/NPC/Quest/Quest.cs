using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    QUEST_VEILED,           // 미해금 상태
    QUEST_OPENED,           // 해금되었으나 미진행 상태 
    QUEST_ONGOING,          // 진행중인 상태 
    QUEST_COMPLETABLE,      // 완료 가능한 상태(퀘스트 달성조건은 충족했으나 NPC와 상호작용 전)    
    QUEST_COMPLETED,        // 완료된 상태 
};

[System.Serializable]
public class Quest 
{
    public int questId;                     // 퀘스트 ID
    public int npcId;                       // NPC ID
    public int precedentId;                 // 선행 퀘스트 ID
    public string title;                    // 퀘스트 제목 
    public string des;                      // 퀘스트 설명
    public string goal;                     // 퀘스트 목표 
    public int type;                        // 퀘스트 타입 
    //public int category;                    // 메인/서브 구분 

    public QuestState state;                    // 퀘스트 진행상태 
    public int exp;                             // 퀘스트 보상 경험치  
    public int gold;                            // 퀘스트 보상 골드 
    public List<RewardItem> rewardItems;        // 퀘스트 보상 아이템 
    public List<RewardKeyword> rewardKeywords;  // 퀘스트 보상 키워드 
}

[System.Serializable]
public class RewardItem
{
    public int itemId;
    public int count; 
}

[System.Serializable]
public class RewardKeyword
{
    public int keywordId;
    public int count;
}

