using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : MonoBehaviour
{
    // 테스트 목적. 이후 JSON -> 테이블 로더를 통해 재구축.
    // Dictionary로 변경 예정.
    //[SerializeField] Item[] items = null; // 임시 테이블
    [SerializeField] Dictionary<int, Quest> questDB = new Dictionary<int, Quest>(); // 실 활용 테이블

    public static QuestDatabase instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddQuest(Quest quest, int questId)
    {
        questDB.Add(questId, quest);
    }

    // 실제 사용
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

    // 임시
    //public Item GetItem(string itemName)
    //{
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        if (items[i].name == itemName)
    //        {
    //            return items[i];
    //        }
    //    }
    //    return null;
    //}
}
