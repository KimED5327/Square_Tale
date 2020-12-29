using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 진행상태 분류 (미해금, 진행가능, 진행중, 완료가능, 완료)
/// </summary>
public enum QuestState
{
    QUEST_VEILED,           // 미해금 상태
    QUEST_OPENED,           // 해금되었으나 미진행 혹은 진행가능 상태 
    QUEST_ONGOING,          // 진행중인 상태 
    QUEST_COMPLETABLE,      // 완료 가능한 상태(퀘스트 달성조건은 충족했으나 NPC와 상호작용 전)    
    QUEST_COMPLETED         // 완료된 상태 
};

/// <summary>
/// 퀘스트 타입 분류 (미해금, 진행가능, 진행중, 완료가능, 완료)
/// </summary>
public enum QuestType
{
    TYPE_DELIVERY,
    TYPE_KILLMONSTER,
    TYPE_COLLECTLOOT,
    TYPE_DIALOGUE
};

/// <summary>
/// 퀘스트 ID, 타입, 진행상태 등 퀘스트와 관련된 모든 정보를 가지고 있는 클래스 
/// </summary>
[System.Serializable]
public class Quest 
{
    int _questID;                     // 퀘스트 ID
    int _npcID;                       // NPC ID
    int _precedentID;                 // 선행 퀘스트 ID
    string _title;                    // 퀘스트 제목 
    string _des;                      // 퀘스트 설명
    string _goal;                     // 퀘스트 목표 

    QuestType _type;                  // 퀘스트 타입 
    QuestState _state;                // 퀘스트 진행상태 
    int _exp;                         // 퀘스트 보상 경험치  
    int _gold;                        // 퀘스트 보상 골드 
    List<BlockUnit> _blockList;       // 퀘스트 보상 블럭 
    List<int> _keywordList;           // 퀘스트 보상 키워드 
    Hashtable _questInfo;             // 퀘스트 타입 상세정보 

    //getter
    public int GetQuestID() { return _questID; }
    public int GetNpcID() { return _npcID; }
    public int GetPrecedentID() { return _precedentID; }
    public string GetTitle() { return _title; }
    public string GetDes() { return _des; }
    public string GetGoal() { return _goal; }

    public QuestType GetQuestType() { return _type; }
    public QuestState GetState() { return _state; }
    public int GetExp() { return _exp; }
    public int GetGold() { return _gold; }
    public List<BlockUnit> GetBlocks() { return _blockList; }
    public List<int> GetKeywords() { return _keywordList; }
    public Hashtable GetQuestInfo() { return _questInfo; }

    //setter 
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetNpcID(int npcID) { _npcID = npcID; }
    public void SetPrecedentID(int precedentID) { _precedentID = precedentID; }
    public void SetTitle(string title) { _title = title; }
    public void SetDes(string des) { _des = des; }
    public void SetGoal(string goal) { _goal = goal; }

    public void SetQuestType(QuestType type) { _type = type; }
    public void SetState(QuestState state) { _state = state; }
    public void SetExp(int exp) { _exp = exp; }
    public void SetGold(int gold) { _gold = gold; }
    public void SetBlocks(List<BlockUnit> blocks) { _blockList = blocks; }
    public void SetKeywords(List<int> keywords) { _keywordList = keywords; }
    public void SetQuestInfo(Hashtable questInfo) { _questInfo = questInfo; }
}

[System.Serializable]
public class ItemUnit
{
    public int itemId;
    public int count; 
}

/// <summary>
/// 퀘스트 보상 블럭 아이템 클래스 (블럭ID, 개수)
/// </summary>
[System.Serializable]
public class BlockUnit
{
    public int _blockID;
    public int _count;

    //getter
    public int GetBlockID() { return _blockID; }
    public int GetCount() { return _count; }

    //setter
    public void SetBlockID(int blockID) { _blockID = blockID; }
    public void SetCount(int count) { _count = count; }
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

