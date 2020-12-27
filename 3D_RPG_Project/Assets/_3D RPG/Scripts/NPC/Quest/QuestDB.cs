using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDB : MonoBehaviour
{
    public static QuestDB instance;
    [SerializeField] Dictionary<int, Quest> questDB = new Dictionary<int, Quest>(); // 실 활용 테이블

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddQuest(Quest quest, int questId)
    {
        questDB.Add(questId, quest);
    }

    // 퀘스트ID에 맞는 퀘스트 반환 
    public Quest GetQuest(int questId)
    {
        Quest quest = questDB[questId];

        if (quest == null) Debug.Log("퀘스트 ID " + questId + "번은 등록되어있지 않습니다.");

        return quest;
    }

    public int GetMaxCount()
    {
        return questDB.Count;
    }
}
