using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 진행상태 분류 (0.미해금 1.진행가능 2.진행중 3.완료가능 4.완료)
/// </summary>
public enum QuestState
{
    /// <summary>
    /// 선행퀘스트를 완료하지 않아서 퀘스트가 미해금된 상태 
    /// </summary>
    QUEST_VEILED,          

    /// <summary>
    /// 퀘스트가 해금되어 진행할 수 있는 상태 
    /// </summary>
    QUEST_OPENED,           

    /// <summary>
    /// 퀘스트를 수락했으나 아직 달성조건을 충족하지 않은 퀘스트 진행중 상태 
    /// </summary>
    QUEST_ONGOING,          

    /// <summary>
    /// 퀘스트의 달성조건을 충족했으나 NPC와 상호작용하기 전인 퀘스트 완료가능 상태 
    /// </summary>
    QUEST_COMPLETABLE,      
    
    /// <summary>
    /// 퀘스트 달성조건을 충족하고 NPC와 상호작용하여 보상이 지급된 퀘스트 완료상태 
    /// </summary>
    QUEST_COMPLETED       
};

/// <summary>
/// 퀘스트 타입 분류 (1.아이템전달 2.전리품획득 3.아이템사용 4.아이템획득 5.오브젝트조작 6.몬스터처치 7.NPC대화)
/// </summary>
public enum QuestType
{
    /// <summary>
    /// 특정한 아이템을 NPC에게 전달하여 완수하는 퀘스트 타입
    /// </summary>
    TYPE_DELIVERITEM = 1,

    /// <summary>
    /// 몬스터를 처치하고 획득한 퀘스트 아이템을 전달하여 완수하는 퀘스트 타입 (퀘스트 아이템은 퀘스트 진행 시에만 드랍)
    /// </summary>
    TYPE_COLLECTLOOT,

    /// <summary>
    /// 몬스터에게 아이템을 사용하여 완수하는 퀘스트 타입 
    /// </summary>
    TYPE_USEITEM,

    /// <summary>
    /// 퀘스트 NPC와 대화 후 특정 아이템을 획득하고 완수되는 퀘스트 타입
    /// </summary>
    TYPE_ACQUIREITEM,

    /// <summary>
    /// 특정 오브젝트를 조작하여 완수하는 퀘스트 타입 (레버를 폭파시켜 다리를 생성하는 퀘스트)
    /// </summary>
    TYPE_OPERATEOBJECT,

    /// <summary>
    /// 특정 몬스터를 지정된 수만큼 처치하여 완수하는 퀘스트 타입
    /// </summary>
    TYPE_KILLENEMY,

    /// <summary>
    /// 특정 NPC와 대화하여 완수하는 퀘스트 타입
    /// </summary>
    TYPE_TALKWITHNPC,
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
    QuestNPC _questGiver;             // 퀘스트 부여자 참조값
    QuestNPC _questFinisher = null;   // 퀘스트 완료자 참조값 
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
    public QuestNPC GetQuestGiver() { return _questGiver; }
    public QuestNPC GetQuestFinisher() { return _questFinisher; }
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
    public void SetQuestGiver(QuestNPC questGiver) { _questGiver = questGiver; }
    public void SetQuestFinisher(QuestNPC questFinisher) { _questFinisher = questFinisher; }
    public void SetExp(int exp) { _exp = exp; }
    public void SetGold(int gold) { _gold = gold; }
    public void SetBlocks(List<BlockUnit> blocks) { _blockList = blocks; }
    public void SetKeywords(List<int> keywords) { _keywordList = keywords; }
    public void SetQuestInfo(Hashtable questInfo) { _questInfo = questInfo; }
}

/// <summary>
/// AcquireItem 퀘스트를 통해 획득하는 아이템 클래스 (아이템 ID, 개수)
/// </summary>
[System.Serializable]
public class ItemUnit
{
    int _itemID;     // 아이템 ID
    int _count;      // 아이템 개수 

    //getter
    public int GetItemID() { return _itemID; }
    public int GetCount() { return _count; }

    //setter
    public void SetItemID(int itemID) { _itemID = itemID; }
    public void SetCount(int count) { _count = count; }
}

/// <summary>
/// 퀘스트 보상 블럭 아이템 클래스 (블럭ID, 개수)
/// </summary>
[System.Serializable]
public class BlockUnit
{
    int _blockID;    // 블럭 ID
    int _count;      // 블럭 개수 

    //getter
    public int GetBlockID() { return _blockID; }
    public int GetCount() { return _count; }

    //setter
    public void SetBlockID(int blockID) { _blockID = blockID; }
    public void SetCount(int count) { _count = count; }
}

/// <summary>
/// 퀘스트의 처치 대상 몬스터 클래스(에너미ID, 퀘스트아이템ID, 개수)
/// </summary>
public class EnemyUnit
{
    int _enemyID;     // 몬스터 ID
    int _itemID;      // 해당 몬스터에서 드랍되는 퀘스트 아이템 ID
    int _count;       // 획득해야 하는 퀘스트 아이템 개수 

    //getter 
    public int GetEnemyID() { return _enemyID; }
    public int GetItemID() { return _itemID; }
    public int GetCount() { return _count; }

    //setter
    public void SetEnemyID(int enemyID) { _enemyID = enemyID; }
    public void SetItemID(int itemID) { _itemID = itemID; }
    public void SetCount(int count) { _count = count; }
}

/// <summary>
/// type1. 특정한 아이템을 NPC에게 전달하여 완수하는 퀘스트 타입 
/// </summary>
public class DeliverItem
{
    int _questID;                                      // 퀘스트 ID
    List<ItemUnit> _itemList = new List<ItemUnit>();   // 전달하는 아이템 리스트 

    //getter
    public int GetQuestID() { return _questID; }
    public List<ItemUnit> GetItemList() { return _itemList; }

    /// <summary>
    /// ItemUnit 리스트에서 idx 순서의 데이터 반환. itemList[idx]
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public ItemUnit GetItem(int idx) { return _itemList[idx]; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetItemList(List<ItemUnit> itemList) { _itemList = itemList; }

    /// <summary>
    /// ItemUnit 리스트에 item을 새 원소로 추가 
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemUnit item) { _itemList.Add(item); }
}

/// <summary>
/// type2. 몬스터를 처치하고 획득한 퀘스트 아이템을 전달하여 완수하는 퀘스트 타입 (퀘스트 아이템은 퀘스트 진행 시에만 드랍)
/// </summary>
public class CollectLoot
{
    int _questID;                                       // 퀘스트 ID
    List<EnemyUnit> _lootList = new List<EnemyUnit>();  // 퀘스트 아이템 리스트  

    //getter
    public int GetQuestID() { return _questID; }
    public List<EnemyUnit> GetLootList() { return _lootList; }

    /// <summary>
    /// EnemyUnit 리스트(lootList)에서 idx 순서의 데이터 반환. lootList[idx]
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public EnemyUnit GetLoot(int idx) { return _lootList[idx]; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetLootList(List<EnemyUnit> lootList) { _lootList = lootList; }

    /// <summary>
    /// EnemyUnit 리스트(lootList)에 loot을 새 원소로 추가  
    /// </summary>
    /// <param name="loot"></param>
    public void AddLoot(EnemyUnit loot) { _lootList.Add(loot); }
}

/// <summary>
/// type3. 몬스터에게 아이템을 사용하여 완수하는 퀘스트 타입 
/// </summary>
public class UseItem
{
    int _questID;                     // 퀘스트 ID
    ItemUnit _item = new ItemUnit();  // 사용하는 아이템  

    //getter
    public int GetQuestID() { return _questID; }
    public ItemUnit GetItem() { return _item; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetItem(ItemUnit item) { _item = item; }
}

/// <summary>
/// type4. 퀘스트 NPC와 대화 후 특정 아이템을 획득하고 완수되는 퀘스트 타입 
/// </summary>
public class AcquireItem
{
    int _questID;                     // 퀘스트 ID
    ItemUnit _item = new ItemUnit();  // 획득하는 아이템 

    //getter
    public int GetQuestID() { return _questID; }
    public ItemUnit GetItem() { return _item; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetItem(ItemUnit item) { _item = item; }
}

/// <summary>
/// type5. 특정 오브젝트를 조작하여 완수하는 퀘스트 타입 (레버를 폭파시켜 다리를 생성하는 퀘스트)
/// </summary>
public class OperateObject
{
    int _questID;       // 퀘스트 ID
    int _objectID;      // 오브젝트 ID

    //getter
    public int GetQuestID() { return _questID; }
    public int GetObjectID() { return _objectID; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetObjectID(int objectID) { _objectID = objectID; }
}

/// <summary>
/// type6. 특정 몬스터를 지정된 수만큼 처치하여 완수하는 퀘스트 타입 
/// </summary>
public class KillEnemy
{
    int _questID;                                        // 퀘스트 ID
    List<EnemyUnit> _enemyList = new List<EnemyUnit>();  // 처치해야 하는 enemy 리스트 

    //getter 
    public int GetQuestID() { return _questID; }
    public List<EnemyUnit> GetEnemyList() { return _enemyList; }
    
    /// <summary>
    /// EnemyUnit 리스트에서 idx 순서의 데이터 반환. enemyList[idx] 
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public EnemyUnit GetEnemy(int idx) { return _enemyList[idx]; }

    //setter 
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetEnemyList(List<EnemyUnit> enemyList) { _enemyList = enemyList; }

    /// <summary>
    /// EnemyUnit 리스트에 enemy를 새 원소로 추가 
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(EnemyUnit enemy) { _enemyList.Add(enemy); }
}

/// <summary>
/// type7. 특정 NPC와 대화하여 완수하는 퀘스트 타입 
/// </summary>
public class TalkWithNpc
{
    int _questID;               // 퀘스트 ID
    int _npcID;                 // NPC ID
    int _firstLineID;           // 해당 NPC와의 대화에서 첫 대사 ID
    int _lastLineID;            // 해당 NPC와의 대화에서 마지막 대사 ID
    QuestNPC _questGiver;       // 퀘스트를 부여하는 NPC 참조값
    QuestNPC _questFinisher;    // 퀘스트를 완료하는 NPC 참조값 

    //getter
    public int GetQuestID() { return _questID; }
    public int GetNpcID() { return _npcID; }
    public int GetFirstLineID() { return _firstLineID; }
    public int GetLastLineID() { return _lastLineID; }
    public QuestNPC GetQuestGiver() { return _questGiver; }
    public QuestNPC GetQuestFinisher() { return _questFinisher; }

    //setter
    public void SetQuestID(int questID) { _questID = questID; }
    public void SetNpcID(int npcID) { _npcID = npcID; }
    public void SetFirstLineID(int firstLineID) { _firstLineID = firstLineID; }
    public void SetLastLineID(int lastLineID) { _lastLineID = lastLineID; }
    public void SetQuestGiver(QuestNPC questGiver) { _questGiver = questGiver; }
    public void SetQuestFinisher(QuestNPC questFinisher) { _questFinisher = questFinisher; }
}

