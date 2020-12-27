using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    QUEST_VEILED,           // 미해금 상태
    QUEST_OPENED,           // 해금되었으나 미진행 상태 
    QUEST_ONGOING,          // 진행중인 상태 
    QUEST_COMPLETABLE,      // 완료 가능한 상태(퀘스트 달성조건은 충족했으나 NPC와 상호작용 전)    
    QUEST_COMPLETED         // 완료된 상태 
};

public enum QuestType
{
    TYPE_DELIVERY,
    TYPE_KILLMONSTER,
    TYPE_COLLECTLOOT,
    TYPE_DIALOGUE
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
    //public int category;                    // 메인/서브 구분 

    public QuestType type;                      // 퀘스트 타입 
    public QuestState state;                    // 퀘스트 진행상태 
    public int exp;                             // 퀘스트 보상 경험치  
    public int gold;                            // 퀘스트 보상 골드 
    public List<ItemUnit> rewardItems;          // 퀘스트 보상 아이템 
    public List<RewardKeyword> rewardKeywords;  // 퀘스트 보상 키워드 
    public Hashtable questInfo;                 // 퀘스트 타입 상세정보 
}

[System.Serializable]
public class ItemUnit
{
    public int itemId;
    public int count; 
}

[System.Serializable]
public class MonsterUnit
{
    public int monsterId;
    //몬스터를 처치하여 특정 아이템을 수집하는 퀘스트에 사용하는 변수 
    public int itemId; 
    public int count; 
}

[System.Serializable]
public class RewardKeyword
{
    public int keywordId;
    public int count;
}

[System.Serializable]
public class Delivery
{
    public int questId; 
    public List<ItemUnit> deliveryItems;
}

[System.Serializable]
public class KillMonster
{
    public int questId; 
    public List<MonsterUnit> monstersToKill;
}

public class CollectLoot
{
    public int questId; 
    public List<MonsterUnit> lootsToCollect; 
}

[System.Serializable]
public class Dialogue
{
    public int questId; 
    public int npcId;
    public int dialogueId; 
}

