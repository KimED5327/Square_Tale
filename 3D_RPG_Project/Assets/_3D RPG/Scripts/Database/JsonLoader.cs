using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class JsonLoader : MonoBehaviour
{
    static readonly string streamingAssetsPath = Application.streamingAssetsPath;

    [SerializeField] string itemDBPath = "/ItemDB.json";
    //[SerializeField] string enemyDBPath = "/EnemyDB.json";


    // Start is called before the first frame update
    void Start()
    {
        // 기획으로부터 DB를 전달받으면 실행.
        // ParsingItemDB();
    }

    // JSON -> DB 파싱
    void ParsingItemDB()
    {
        // jsonData Get
        string path = streamingAssetsPath + itemDBPath;
        JsonData jData = GetJsonData(path);

        // 모든 Row 순회
        for (int i = 0; i < jData.Count; i++)
        {
            Item item = new Item();

            string name = jData[i][0].ToString();
            string type = jData[i][1].ToString(); // 아이템 분류 기획이 나오면 수정
            string desc = jData[i][2].ToString();
            int price = int.Parse(jData[i][3].ToString());

            // 옵션 필드가 하나라도 있다면
            List<Option> optionList = new List<Option>();
            if (jData[i][4].Count > 0)
            {
                // 옵션 추가
                for (int k = 0; k < jData[i][4].Count; k++)
                {
                    Option option = new Option
                    {
                        name = jData[i][4][k].ToString(),
                        num = float.Parse(jData[i][5][k].ToString())
                    };
                    optionList.Add(option);
                }
            }

            item.name = name;
            item.desc = desc;
            item.price = price;
            item.options = optionList;
            item.type = ItemType.WEAPON;
            ItemDatabase.instance.AddItem(item, name);
        }
    }

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
        // 피시
        else
            jsonString = File.ReadAllText(path);

        return JsonMapper.ToObject(jsonString);
    }
}
