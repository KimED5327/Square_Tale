using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class QuestLoader : MonoBehaviour
{
    public static QuestLoader instance; 
    static string streamingAssetsPath = Application.streamingAssetsPath;
    string questInfoKey = "info";

    // 퀘스트 정보 DB 경로 
    [SerializeField] string questDBPath;

    // 퀘스트 타입별 DB 경로 
    [SerializeField] string deliverItemDBPath;
    [SerializeField] string collectLootDBPath;
    [SerializeField] string useItemDBPath;
    [SerializeField] string carryItemDBPath;
    [SerializeField] string operateObjectDBPath;
    [SerializeField] string killEnemyDBPath;
    [SerializeField] string talkWithNpcDBPath;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    void Start()
    {
        // 퀘스트 기본정보 파싱 
        ParsingQuestDB();
        //PrintQuestInfo();

        // 퀘스트 타입별 상세정보 파싱 
        ParsingQuestTypeDB();
        //PrintQuestTypeInfo();
    }

    /// <summary>
    /// 퀘스트 정보 DB 데이터 파싱 
    /// </summary>
    private void ParsingQuestDB()
    {
        string path = streamingAssetsPath + questDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        // 모든 row 순회 
        for (int i = 0; i < jData.Count; i++)
        {
            Quest quest = new Quest();

            int questID = int.Parse(jData[i][0].ToString());
            int npcID = int.Parse(jData[i][1].ToString());
            int precedentID = int.Parse(jData[i][2].ToString());
            string title = jData[i][3].ToString();
            string des = jData[i][4].ToString();
            string goal = jData[i][5].ToString();

            //int형 밸류는 enum형 변수에 대입이 되지 않기 때문에 enum 타입으로 캐스팅 
            QuestType type = (QuestType)int.Parse(jData[i][6].ToString());
            QuestState state = (QuestState)int.Parse(jData[i][7].ToString());
            int exp = int.Parse(jData[i][8].ToString());
            int gold = int.Parse(jData[i][9].ToString());

            // 블럭 필드가 비어있지 않은 경우에만 값을 대입 
            // 블럭 필드가 배열(값이 2개 이상)인지 아닌지 확인하여 값을 대입 
            List<BlockUnit> blockList = new List<BlockUnit>();
            if(!JsonManager.instance.IsNullString(jData[i][10].ToString()))
            {
                // 블럭 필드가 배열이 아닌 단일값인 경우 
                if(!jData[i][10].IsArray)
                {
                    BlockUnit block = new BlockUnit();

                    block.SetBlockID(int.Parse(jData[i][10].ToString()));
                    block.SetCount(int.Parse(jData[i][11].ToString()));
                    blockList.Add(block);
                }
                else
                {
                    for (int j = 0; j < jData[i][10].Count; j++)
                    {
                        // 원소가 빈 문자열일 경우 for문 종료 
                        if (JsonManager.instance.IsNullString(jData[i][10][j].ToString())) break;

                        BlockUnit block = new BlockUnit();

                        block.SetBlockID(int.Parse(jData[i][10][j].ToString()));
                        block.SetCount(int.Parse(jData[i][11][j].ToString()));
                        blockList.Add(block);
                    }
                }
            }

            // 키워드 필드가 비어있지 않은 경우에만 값을 대입 
            // 키워드 필드가 배열(값이 2개 이상)인지 아닌지 확인하여 값을 대입 
            List<int> keywordList = new List<int>();
            if(!JsonManager.instance.IsNullString(jData[i][12].ToString()))
            {
                // 키워드 필드가 배열이 아닌 단일값일 경우 
                if (!jData[i][12].IsArray)
                {
                    keywordList.Add(int.Parse(jData[i][12].ToString()));
                }
                else
                {
                    for (int j = 0; j < jData[i][12].Count; j++)
                    {
                        // 원소가 빈 문자열일 for문 종료 
                        if (JsonManager.instance.IsNullString(jData[i][12][j].ToString())) break; 

                        keywordList.Add(int.Parse(jData[i][12][j].ToString()));
                    }
                }
            }

            // 값 대입 
            quest.SetQuestID(questID);
            quest.SetNpcID(npcID);
            quest.SetPrecedentID(precedentID);
            quest.SetTitle(title);
            quest.SetDes(des);
            quest.SetGoal(goal);

            quest.SetQuestType(type);
            quest.SetState(state);
            quest.SetExp(exp);
            quest.SetGold(gold);
            quest.SetBlocks(blockList);
            quest.SetKeywords(keywordList);

            QuestDB.instance.AddQuest(quest, questID);
            //Debug.Log(questID + "번째 퀘스트 추가");
        }
    }

    /// <summary>
    /// 퀘스트 타입별 DB 데이터 파싱 
    /// </summary>
    private void ParsingQuestTypeDB()
    {
        ParsingDeliverItemDB();
        //ParsingCollectLootDB();
        //ParsingUseItemDB();
        ParsingCarryItemDB();
        //ParsingOperateObjectDB();
        //ParsingKillEnemyDB();
        ParsingTalkWithNpc();
    }

    /// <summary>
    /// type1. DeliverItem 타입 퀘스트 DB 파싱  
    /// </summary>
    private void ParsingDeliverItemDB()
    {
        string path = streamingAssetsPath + deliverItemDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            DeliverItem deliverItem = new DeliverItem();

            int questID = int.Parse(jData[i][0].ToString());

            // 단일 변수일 경우 
            if (!jData[i][1].IsArray)
            {
                ItemUnit item = new ItemUnit();

                item.SetItemID(int.Parse(jData[i][1].ToString()));
                item.SetCount(int.Parse(jData[i][2].ToString()));
                deliverItem.AddItem(item);
            }
            else
            {
                for (int j = 0; j < jData[i][1].Count; j++)
                {
                    ItemUnit item = new ItemUnit();

                    item.SetItemID(int.Parse(jData[i][1][j].ToString()));
                    item.SetCount(int.Parse(jData[i][2][j].ToString()));
                    deliverItem.AddItem(item);
                }
            }

            deliverItem.SetQuestID(questID);
            questInfo.Add(questInfoKey, deliverItem);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }        
    }

    /// <summary>
    /// type2. CollectLoot 타입 퀘스트 DB 파싱 
    /// </summary>
    private void ParsingCollectLootDB()
    {
        string path = streamingAssetsPath + collectLootDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            CollectLoot collectLoot = new CollectLoot();

            int questID = int.Parse(jData[i][0].ToString());

            // 단일 변수일 경우 
            if(!jData[i][1].IsArray)
            {
                EnemyUnit enemy = new EnemyUnit();

                enemy.SetEnemyID(int.Parse(jData[i][1].ToString()));
                enemy.SetItemID(int.Parse(jData[i][2].ToString()));
                enemy.SetCount(int.Parse(jData[i][3].ToString()));
                collectLoot.GetLootList().Add(enemy);
            }
            else
            {
                for (int j = 0; j < jData[i][1].Count; j++)
                {
                    EnemyUnit enemy = new EnemyUnit();

                    enemy.SetEnemyID(int.Parse(jData[i][1][j].ToString()));
                    enemy.SetItemID(int.Parse(jData[i][2][j].ToString()));
                    enemy.SetCount(int.Parse(jData[i][3][j].ToString()));
                    collectLoot.GetLootList().Add(enemy);
                }
            }

            collectLoot.SetQuestID(questID);
            questInfo.Add(questInfoKey, collectLoot);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    /// <summary>
    /// type3. UseItem 타입 퀘스트 DB 파싱 
    /// </summary>
    private void ParsingUseItemDB()
    {
        string path = streamingAssetsPath + useItemDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            UseItem useItem = new UseItem();

            int questID = int.Parse(jData[i][0].ToString());
            useItem.GetItem().SetItemID(int.Parse(jData[i][1].ToString()));
            useItem.GetItem().SetCount(int.Parse(jData[i][2].ToString()));

            useItem.SetQuestID(questID);
            questInfo.Add(questInfoKey, useItem);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    /// <summary>
    /// type4. CarryItem 타입 퀘스트 DB 파싱 
    /// </summary>
    private void ParsingCarryItemDB()
    {
        string path = streamingAssetsPath + carryItemDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            CarryItem carryItem = new CarryItem();

            int questID = int.Parse(jData[i][0].ToString());
            carryItem.GetItem().SetItemID(int.Parse(jData[i][1].ToString()));
            carryItem.GetItem().SetCount(int.Parse(jData[i][2].ToString()));

            carryItem.SetQuestID(questID);
            questInfo.Add(questInfoKey, carryItem);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    /// <summary>
    /// type5. OperateObject 타입 퀘스트 DB 파싱 
    /// </summary>
    private void ParsingOperateObjectDB()
    {
        string path = streamingAssetsPath + operateObjectDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            OperateObject operateObject = new OperateObject();

            int questID = int.Parse(jData[i][0].ToString());

            operateObject.SetQuestID(questID);
            operateObject.SetObjectID(int.Parse(jData[i][1].ToString()));
            questInfo.Add(questInfoKey, operateObject);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    /// <summary>
    /// type6. KillEnemy 타입 퀘스트 DB 파싱 
    /// </summary>
    private void ParsingKillEnemyDB()
    {
        string path = streamingAssetsPath + killEnemyDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            KillEnemy killEnemy = new KillEnemy();

            int questID = int.Parse(jData[i][0].ToString());

            // 단일 변수일 경우 
            if(!jData[i][1].IsArray)
            {
                EnemyUnit enemy = new EnemyUnit();

                enemy.SetEnemyID(int.Parse(jData[i][1].ToString()));
                enemy.SetCount(int.Parse(jData[i][2].ToString()));
                killEnemy.GetEnemyList().Add(enemy);
            }
            else
            {
                for (int j = 0; j < jData[i][1].Count; j++)
                {
                    EnemyUnit enemy = new EnemyUnit();

                    enemy.SetEnemyID(int.Parse(jData[i][1][j].ToString()));
                    enemy.SetCount(int.Parse(jData[i][2][j].ToString()));
                    killEnemy.GetEnemyList().Add(enemy);
                }
            }

            killEnemy.SetQuestID(questID);
            questInfo.Add(questInfoKey, killEnemy);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    /// <summary>
    /// type7. TalkWithNpc 타입 퀘스트 DB 파싱
    /// </summary>
    private void ParsingTalkWithNpc()
    {
        string path = streamingAssetsPath + talkWithNpcDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            TalkWithNpc talkWithNpc = new TalkWithNpc();

            int questID = int.Parse(jData[i][0].ToString());
            talkWithNpc.SetQuestID(questID);
            talkWithNpc.SetNpcID(int.Parse(jData[i][1].ToString()));
            talkWithNpc.SetFirstLineID(int.Parse(jData[i][2].ToString()));
            talkWithNpc.SetLastLineID(int.Parse(jData[i][3].ToString()));

            questInfo.Add(questInfoKey, talkWithNpc);
            QuestDB.instance.GetQuest(questID).SetQuestInfo(questInfo);
        }
    }

    void PrintQuestInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            Debug.Log((i + 1) + "번째 퀘스트 제목 : " + QuestDB.instance.GetQuest(i + 1).GetTitle());

            for (int j = 0; j < QuestDB.instance.GetQuest(i + 1).GetBlocks().Count; j++)
            {
                Debug.Log((j + 1) + "번째 블럭 번호와 수량 ( " +
                    QuestDB.instance.GetQuest(i + 1).GetBlocks()[j].GetBlockID() + ", " +
                    QuestDB.instance.GetQuest(i + 1).GetBlocks()[j].GetCount() + " )");
            }
        }
    }

    void PrintQuestTypeInfo()
    {
        //PrintDeliverItemInfo();
        //PrintCollectLootInfo();
        //PrintUseItemInfo();
        //PrintCarryItemInfo();
        //PrintKillEnemyInfo();
        //PrintTalkWithNpcInfo();
    }

    void PrintDeliverItemInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_DELIVERITEM)
            {
                DeliverItem deliverItem = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()["info"] as DeliverItem;

                for (int j = 0; j < deliverItem.GetItemList().Count; j++)
                {
                    Debug.Log(deliverItem.GetQuestID() + "번째 퀘스트 " +
                        deliverItem.GetItem(j).GetItemID() + "번 아이템 " +
                        deliverItem.GetItem(j).GetCount() + "개");
                }
            }
        }
    }

    void PrintCollectLootInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_COLLECTLOOT)
            {
                CollectLoot questInfo = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()["info"] as CollectLoot;

                for (int j = 0; j < questInfo.GetLootList().Count; j++)
                {
                    Debug.Log(questInfo.GetQuestID() + "번째 퀘스트 " +
                        questInfo.GetLoot(j).GetEnemyID() + "번 몬스터 " +
                        questInfo.GetLoot(j).GetItemID() + "번 아이템 " +
                        questInfo.GetLoot(j).GetCount() + "개");
                }
            }
        }
    }

    void PrintUseItemInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_USEITEM)
            {
                UseItem questInfo = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()[questInfoKey] as UseItem;

                Debug.Log(questInfo.GetQuestID() + "번째 퀘스트 " +
                     questInfo.GetItem().GetItemID() + "번 아이템 " +
                     questInfo.GetItem().GetCount() + "개");
            }
        }
    }

    void PrintCarryItemInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_CARRYITEM)
            {
                CarryItem questInfo = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()[questInfoKey] as CarryItem;

                Debug.Log(questInfo.GetQuestID() + "번째 퀘스트 " +
                     questInfo.GetItem().GetItemID() + "번 아이템 " +
                     questInfo.GetItem().GetCount() + "개");
            }
        }
    }

    void PrintKillEnemyInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_KILLENEMY)
            {
                KillEnemy questInfo = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()[questInfoKey] as KillEnemy;

                for (int j = 0; j < questInfo.GetEnemyList().Count; j++)
                {
                    Debug.Log(questInfo.GetQuestID() + "번째 퀘스트 " +
                        questInfo.GetEnemy(j).GetEnemyID() + "번 몬스터 " +
                        questInfo.GetEnemy(j).GetCount() + "마리");
                }
            }
        }
    }

    void PrintTalkWithNpcInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if (QuestDB.instance.GetQuest(i + 1).GetQuestType() == QuestType.TYPE_TALKWITHNPC)
            {
                TalkWithNpc questInfo = QuestDB.instance.GetQuest(i + 1).GetQuestInfo()[questInfoKey] as TalkWithNpc;

                Debug.Log(questInfo.GetQuestID() + "번째 퀘스트 " +
                     questInfo.GetNpcID() + "번 NPC " +
                     "첫번째 대사 " + questInfo.GetFirstLineID() + "번" +
                     "마지막 대사 " + questInfo.GetLastLineID() + "번");
            }
        }
    }
}
