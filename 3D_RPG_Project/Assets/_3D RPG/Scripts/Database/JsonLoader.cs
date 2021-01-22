using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class JsonLoader : MonoBehaviour
{
    static string streamingAssetsPath = Application.streamingAssetsPath;

    [SerializeField] string itemDBPath = "/ItemDB.json";
    //[SerializeField] string enemyDBPath = "/EnemyDB.json";


    // Start is called before the first frame update
    void OnEnable()
    {
        ParsingItemDB();
    }

    // JSON -> DB 파싱
    void ParsingItemDB()
    {

        string jsonString;
        string path = streamingAssetsPath + itemDBPath;
        if(Application.platform == RuntimePlatform.Android){
            WWW reader = new WWW(path);
            while (!reader.isDone)
            {

            }
            jsonString = reader.text;
        }
        else
        {
            jsonString = File.ReadAllText(path);
        }

        JsonData jData = JsonMapper.ToObject(jsonString);

        // 모든 Row 순회
        for (int i = 0; i < jData.Count; i++)
        {
            Item item = new Item();

            int id = int.Parse(jData[i][0].ToString());
            string name = jData[i][1].ToString();
            string iconName = jData[i][2].ToString();
            int type = int.Parse(jData[i][3].ToString());
            var itemType = (ItemType)type;

            ItemCategory itemCategory;
            if (itemType == ItemType.WEAPON || itemType == ItemType.STAFF)
                itemCategory = ItemCategory.WEAPON;
            else if (itemType == ItemType.ARMOR || itemType == ItemType.SUDAN)
                itemCategory = ItemCategory.ARMOR;
            else
                itemCategory = ItemCategory.ETC;

            int stack = int.Parse(jData[i][4].ToString());
            bool stackable = (stack > 1);
            int canSaleInt = int.Parse(jData[i][5].ToString());
            int priceBuy = int.Parse(jData[i][6].ToString());
            int priceSell = int.Parse(jData[i][7].ToString());
            
            List<Option> optionList = new List<Option>();
            for (int k = 8; k < 12; k++)
            {
                if (jData[i][k] != null)
                {
                    var t_type = OptionType.HP;  
                    switch (k)
                    {
                        case 8: t_type = OptionType.HP; break;
                        case 9 : t_type = OptionType.STR; break;
                        case 10: t_type = OptionType.INT; break;
                        case 11: t_type = OptionType.DEF; break;
                        case 12: t_type = OptionType.SPEED; break;
                    };

                    Option option = new Option();
                    option.opType = t_type;
                    option.num = float.Parse(jData[i][k].ToString());
                    
                    optionList.Add(option);
                }
            }

            string desc = jData[i][13].ToString();

            item.id = id;
            item.name = name;
            item.stack = stack;
            item.iconName = iconName;
            item.stackable = stackable;
            item.desc = desc;
            item.priceBuy = priceBuy;
            item.priceSell = priceSell;
            item.canSell = canSaleInt == 2;
            item.options = optionList;
            item.type = itemType;
            item.category = itemCategory;

            ItemDatabase.instance.AddItem(item, id);
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