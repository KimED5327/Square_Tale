using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class QuestLoader : MonoBehaviour
{
    static readonly string streamingAssetsPath = Application.streamingAssetsPath;

    [SerializeField] string questDBPath = "/questDB_test.json";
    //[SerializeField] string questDBPath = "/data.json";
    //[SerializeField] string enemyDBPath = "/EnemyDB.json";

    // Start is called before the first frame update
    void Start()
    {
        // 기획으로부터 DB를 전달받으면 실행.
        //ParsingQuestDB();

        //Debug.Log(QuestDatabase.instance.GetMaxCount());

        //printQuestInfo();
    }

    // JSON -> DB 파싱
    private void ParsingQuestDB()
    {
        // jsonData get
        string path = streamingAssetsPath + questDBPath;
        JsonData jData = GetJsonData(path);

        //Quest quest = new Quest();

        //QuestState state = (QuestState)int.Parse(jData[0][7].ToString());

        //List<RewardItem> itemList = new List<RewardItem>();
        //if (jData[0][10].Count > 0)
        //{
        //    // 아이템 추가
        //    for (int j = 0; j < jData[0][10].Count; j++)
        //    {
        //        RewardItem rewardItem = new RewardItem();

        //        rewardItem.itemId = int.Parse(jData[0][10][j].ToString());
        //        rewardItem.count = int.Parse(jData[0][11][j].ToString());

        //        itemList.Add(rewardItem);
        //    }
        //}

        //quest.state = state;
        //quest.rewardItems = itemList;

        //QuestDatabase.instance.AddQuest(quest, 1);

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
            int type = int.Parse(jData[i][6].ToString());

            //int형 밸류는 enum형 변수에 대입이 되지 않기 때문에 enum 타입으로 캐스팅 
            QuestState state = (QuestState)int.Parse(jData[i][7].ToString());
            int exp = int.Parse(jData[i][8].ToString());
            int gold = int.Parse(jData[i][9].ToString());

            // 아이템 필드가 하나라도 있다면
            List<RewardItem> itemList = new List<RewardItem>();
            if (jData[i][10].Count > 0)
            {
                // 아이템 추가
                for (int j = 0; j < jData[i][10].Count; j++)
                {
                    RewardItem rewardItem = new RewardItem();

                    rewardItem.itemId = int.Parse(jData[i][10][j].ToString());
                    rewardItem.count = int.Parse(jData[i][11][j].ToString());

                    itemList.Add(rewardItem);
                }
            }

            // keyword 필드가 하나라도 있다면 
            List<RewardKeyword> keywordList = new List<RewardKeyword>();
            if (jData[i][12].Count > 0)
            {
                // 아이템 추가
                for (int j = 0; j < jData[i][12].Count; j++)
                {
                    RewardKeyword rewardKeyword = new RewardKeyword();

                    rewardKeyword.keywordId = int.Parse(jData[i][12][j].ToString());
                    rewardKeyword.count = int.Parse(jData[i][13][j].ToString());

                    keywordList.Add(rewardKeyword);
                }
            }

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
            quest.rewardItems = itemList;
            quest.rewardKeywords = keywordList;

            QuestDatabase.instance.AddQuest(quest, questId);
            Debug.Log((i + 1) + "번째 퀘스트 추가");
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
        for (int i = 0; i < QuestDatabase.instance.GetMaxCount(); i++)
        {
            //Debug.Log(QuestDatabase.instance.GetQuest(i + 1).questId +
            //    "번째 퀘스트 제목 : " + QuestDatabase.instance.GetQuest(i + 1).title);

            Debug.Log("진행상태 : " + QuestDatabase.instance.GetQuest(i + 1).state);

            for (int j = 0; j < QuestDatabase.instance.GetQuest(i + 1).rewardItems.Count; j++)
            {
                Debug.Log((j + 1) + "번째 아이템 번호와 수량 (" +
                    QuestDatabase.instance.GetQuest(i + 1).rewardItems[j].itemId + ", " +
                    QuestDatabase.instance.GetQuest(i + 1).rewardItems[j].count + " )");
            }
        }
    }
}
