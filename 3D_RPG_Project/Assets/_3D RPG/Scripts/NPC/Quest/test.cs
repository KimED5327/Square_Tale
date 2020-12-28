using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Hashtable hash = new Hashtable();
    Quest quest = new Quest();

    // Start is called before the first frame update
    void Start()
    {
        //printQuestInfo();
        AddData();
        printData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void printQuestInfo()
    {
        for (int i = 0; i < QuestDB.instance.GetMaxCount(); i++)
        {
            Debug.Log((i + 1) + "번째 퀘스트 제목 : " + QuestDB.instance.GetQuest(i + 1).GetTitle());
        }
    }

    void AddData()
    {
        quest.SetTitle("테스트 퀘스트");

        hash.Add("quest", quest);
    }

    void printData()
    {
        if (hash.ContainsKey("quest"))
        {
            Quest temp = hash["quest"] as Quest;

            Debug.Log(temp.GetTitle());
        }
    }
}
