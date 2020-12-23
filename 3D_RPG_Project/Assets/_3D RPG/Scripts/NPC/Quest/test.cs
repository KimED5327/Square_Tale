using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        printQuestInfo();

        //Debug.Log("Method working");

        //Debug.Log(QuestDatabase.instance.GetMaxCount());

        //for (int i = 0; i < QuestDatabase.instance.GetMaxCount(); i++)
        //{
        //    Debug.Log((i + 1) + "번째 퀘스트 제목 : " + QuestDatabase.instance.GetQuest(i + 1).title);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void printQuestInfo()
    {
        for(int i=0; i<QuestDatabase.instance.GetMaxCount(); i++)
        {
            Debug.Log((i + 1) + "번째 퀘스트 제목 : " + QuestDatabase.instance.GetQuest(i + 1).title);
        }
    }
}
