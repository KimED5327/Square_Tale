using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class QuestLoader : MonoBehaviour
{
    static readonly string streamingAssetsPath = Application.streamingAssetsPath;

    [SerializeField] string questDBPath;
    [SerializeField] string deliveryDBPath;
    [SerializeField] string killMonsterDBPath;
    [SerializeField] string collectLootDBPath;
    [SerializeField] string dialogueDBPath;
    //[SerializeField] string questDBPath = "/data.json";

    // Start is called before the first frame update
    void Start()
    {
        // 퀘스트 기본정보 파싱 
        ParsingQuestDB();

        // 퀘스트 타입별 상세정보 파싱 
        ParsingQuestTypeDB();

        //printQuestInfo();
        //printQuestTypeInfo();
    }

    // JSON -> DB 파싱
    private void ParsingQuestDB()
    {
        // jsonData get
        string path = streamingAssetsPath + questDBPath;
        JsonData jData = GetJsonData(path);

        // 모든 row 순회 
        for (int i = 0; i < jData.Count; i++)
        {
            Quest quest = new Quest();

            int questId = int.Parse(jData[i][0].ToString());
            int npcId = int.Parse(jData[i][1].ToString());
            int precedentId = int.Parse(jData[i][2].ToString());
            string title = jData[i][3].ToString();
            string des = jData[i][4].ToString();
            string goal = jData[i][5].ToString();

            //int형 밸류는 enum형 변수에 대입이 되지 않기 때문에 enum 타입으로 캐스팅 
            QuestType type = (QuestType)int.Parse(jData[i][6].ToString());
            QuestState state = (QuestState)int.Parse(jData[i][7].ToString());
            int exp = int.Parse(jData[i][8].ToString());
            int gold = int.Parse(jData[i][9].ToString());

            // 아이템 필드가 하나라도 있다면
            List<ItemUnit> rewardList = new List<ItemUnit>();
            if (!IsNullString(jData[i][10].ToString()) && jData[i][10].Count > 0)
            {
                // 아이템 추가
                for (int j = 0; j < jData[i][10].Count; j++)
                {
                    if (IsNullString(jData[i][10][j].ToString())) break;

                    ItemUnit rewardItem = new ItemUnit();

                    rewardItem.itemId = int.Parse(jData[i][10][j].ToString());
                    rewardItem.count = int.Parse(jData[i][11][j].ToString());

                    rewardList.Add(rewardItem);
                }
            }

            // keyword 필드가 하나라도 있다면 
            //List<RewardKeyword> keywordList = new List<RewardKeyword>();
            //if (jData[i][12].Count > 0)
            //{
            //    // 키워드 추가
            //    for (int j = 0; j < jData[i][12].Count; j++)
            //    {
            //        RewardKeyword rewardKeyword = new RewardKeyword();

            //        rewardKeyword.keywordId = int.Parse(jData[i][12][j].ToString());
            //        rewardKeyword.count = int.Parse(jData[i][13][j].ToString());

            //        keywordList.Add(rewardKeyword);
            //    }
            //}

            quest.questId = questId;
            quest.npcId = npcId;
            quest.precedentId = precedentId;
            quest.title = title;
            quest.des = des;
            quest.goal = goal;

            quest.type = type;
            quest.state = state;
            quest.exp = exp;
            quest.gold = gold;
            quest.rewardItems = rewardList;
            //quest.rewardKeywords = keywordList;

            QuestDB.instance.AddQuest(quest, questId);
            //Debug.Log((i + 1) + "번째 퀘스트 추가");
        }
    }

    void ParsingQuestTypeDB()
    {
        ParsingDeliveryDB();
    }

    void ParsingDeliveryDB()
    {
        string path = streamingAssetsPath + deliveryDBPath;
        JsonData jData = GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            Hashtable questInfo = new Hashtable();
            Delivery delivery = new Delivery();

            int questId = int.Parse(jData[i][0].ToString());

            List<ItemUnit> deliveryList = new List<ItemUnit>();
            for (int j = 0; j < jData[i][1].Count; j++)
            {
                //비어있는 항목을 만나면 반복문 종료 
                if (IsNullString(jData[i][1][j].ToString())) break;

                ItemUnit deliveryItem = new ItemUnit();

                deliveryItem.itemId = int.Parse(jData[i][1][j].ToString());
                deliveryItem.count = int.Parse(jData[i][2][j].ToString());

                deliveryList.Add(deliveryItem);
            }

            delivery.questId = questId;
            delivery.deliveryItems = deliveryList;
            questInfo.Add("info", delivery);

            QuestDB.instance.GetQuest(questId).questInfo = questInfo;
            //QuestDatabase.instance.GetQuest(questId).questInfo.Add("info", delivery);
        }
    }

    //void ParsingItemDB()
    //{
    //    // jsonData Get
    //    string path = streamingAssetsPath + itemDBPath;
    //    JsonData jData = GetJsonData(path);

    //    // 모든 Row 순회
    //    for (int i = 0; i < jData.Count; i++)
    //    {
    //        Item item = new Item();

    //        string name = jData[i][0].ToString();
    //        string type = jData[i][1].ToString(); // 아이템 분류 기획이 나오면 수정
    //        string desc = jData[i][2].ToString();
    //        int price = int.Parse(jData[i][3].ToString());

    //        // 옵션 필드가 하나라도 있다면
    //        List<Option> optionList = new List<Option>();
    //        if (jData[i][4].Count > 0)
    //        {
    //            // 옵션 추가
    //            for (int k = 0; k < jData[i][4].Count; k++)
    //            {
    //                Option option = new Option
    //                {
    //                    name = jData[i][4][k].ToString(),
    //                    num = float.Parse(jData[i][5][k].ToString())
    //                };
    //                optionList.Add(option);
    //            }
    //        }

    //        item.name = name;
    //        item.desc = desc;
    //        item.price = price;
    //        item.options = optionList;
    //        item.type = ItemType.WEAPON;
    //        ItemDatabase.instance.AddItem(item, name);
    //    }
    //}

    // JsonString -> JsonData 변환 

    JsonData GetJsonData(string path)
    {
        string jsonString = "";

        // 안드로이드 
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest reader = new UnityWebRequest(path);
            while (!reader.isDone)
                jsonString = reader.downloadHandler.text;
        }
        else // PC
        {
            jsonString = File.ReadAllText(path);
        }

        return JsonMapper.ToObject(jsonString);
    }

    void printQuestInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            //Debug.Log(QuestDatabase.instance.GetQuest(i + 1).questId +
            //    "번째 퀘스트 제목 : " + QuestDatabase.instance.GetQuest(i + 1).title);

            Debug.Log("진행상태 : " + QuestDB.instance.GetQuest(i + 1).state);

            for (int j = 0; j < QuestDB.instance.GetQuest(i + 1).rewardItems.Count; j++)
            {
                Debug.Log((j + 1) + "번째 아이템 번호와 수량 (" +
                    QuestDB.instance.GetQuest(i + 1).rewardItems[j].itemId + ", " +
                    QuestDB.instance.GetQuest(i + 1).rewardItems[j].count + " )");
            }
        }
    }

    void printQuestTypeInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            if(QuestDB.instance.GetQuest(i+1).type == QuestType.TYPE_DELIVERY)
            {
                Delivery delivery = QuestDB.instance.GetQuest(i + 1).questInfo["info"] as Delivery;

                for (int j = 0; j < delivery.deliveryItems.Count; j++)
                {
                    Debug.Log(delivery.questId + "번째 퀘스트 " +
                        delivery.deliveryItems[j].itemId + "번 아이템 " +
                        delivery.deliveryItems[j].count + "개");

                    int[] arr = new int[5];
                    arr[(int)QuestState.QUEST_COMPLETED - 1] = 5;
                }
            }
        }
    }

    // 넘겨받은 문자열이 NULL이나 빈 문자열인지 판별 
    private bool IsNullString(string str)
    {
        return string.IsNullOrEmpty(str);
    }
}
