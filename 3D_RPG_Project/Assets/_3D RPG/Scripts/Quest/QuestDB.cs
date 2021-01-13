using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 데이터베이스 (int 타입 questID를 key, Quest 클래스를 value로 Dictionary 컬렉션에서 데이터 관리)
/// </summary>
public class QuestDB : MonoBehaviour
{
    public static QuestDB instance;
    [SerializeField] Dictionary<int, Quest> questDB = new Dictionary<int, Quest>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// questID key, Quest value를 가진 데이터를 데이터베이스 Dictionary에 저장 
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="questID"></param>
    public void AddQuest(Quest quest, int questID)
    {
        questDB.Add(questID, quest);
    }

    /// <summary>
    /// questID key값에 맞는 Quest 데이터를 반환 
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public Quest GetQuest(int questID)
    {
        Quest quest = questDB[questID];

        if (quest == null) Debug.Log("퀘스트 ID " + questID + "번은 등록되어있지 않습니다.");

        return questDB[questID];
    }

    /// <summary>
    /// QuestDB 내 전체 데이터의 개수를 반환 
    /// </summary>
    /// <returns></returns>
    public int GetMaxCount()
    {
        return questDB.Count;
    }
}
