using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    [SerializeField]
    Dictionary<string, ItemData> itemDB = new Dictionary<string, ItemData>();

    private void Start()
    {
        string jsonString = "";
        string path = Application.streamingAssetsPath + "/ItemDB.json";

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

        JsonData jData = JsonMapper.ToObject(jsonString);
        

        for(int i = 0; i < jData.Count; i++)
        {
            ItemData item;

            string name = jData[i][0].ToString();
            string type = jData[i][1].ToString();
            string desc = jData[i][2].ToString();
            int value = int.Parse(jData[i][3].ToString());

            Debug.Log(name + " " + desc + " " + value);


            List<Option> optionList = new List<Option>();
            if(jData[i][4].Count > 0)
            {
                for(int k = 0; k < jData[i][4].Count; k++)
                {
                    Option option;
                    option.name = jData[i][4][k].ToString();
                    option.num = float.Parse(jData[i][5][k].ToString());
                    Debug.Log(option.name + " " + option.num);
                    optionList.Add(option);
                }
            }

            item.name = name;
            item.desc = desc;
            item.value = value;
            item.options = optionList;
            item.type = type;
            itemDB.Add(name, item);
        }
    }


    [System.Serializable]
    public struct ItemData
    {
        public string name;
        public string type;
        public string desc;
        public int value;
        public List<Option> options;
    }

    [System.Serializable]
    public struct Option
    {
        public string name;
        public float num;
    }

    public enum ItemType
    {
        CONSUMABLE,
        INGREDIENT,
        QUEST,
        ETC,
        CASH,
        ARMOR,
        WEAPON,
        HELMET,
        BOOT,
        GLOVE,
    }
}
