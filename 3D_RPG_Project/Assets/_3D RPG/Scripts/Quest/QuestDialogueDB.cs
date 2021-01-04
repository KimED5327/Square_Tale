using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 다이얼로그 데이터베이스 (int 타입 questID를 key, QuestDialogue 클래스를 value로 Dictionary 컬렉션에서 데이터 관리)
/// </summary>
public class QuestDialogueDB : MonoBehaviour
{
    public static QuestDialogueDB instance;
    Dictionary<int, QuestDialogue> questDialogueDB = new Dictionary<int, QuestDialogue>();

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    /// <summary>
    /// questID key, QuestDialogue value를 가진 데이터를 데이터베이스 Dictionary에 추가 
    /// </summary>
    /// <param name="questID"></param>
    /// <param name="questDialogue"></param>
    public void AddDialogue(int questID, QuestDialogue questDialogue)
    {
        questDialogueDB.Add(questID, questDialogue);
    }

    /// <summary>
    /// questID key값에 맞는 QuestDialogue 데이터를 반환 
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public QuestDialogue GetDialogue(int questID)
    {
        QuestDialogue questDialouge = questDialogueDB[questID];

        if (questDialouge == null) Debug.Log("퀘스트 ID " + questID + "번은 등록되어있지 않습니다.");

        return questDialouge;
    }

    /// <summary>
    /// 해당 questID를 key로 가진 데이터 유무를 bool 타입으로 반환 
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public bool CheckKey(int questID)
    {
        return questDialogueDB.ContainsKey(questID);
    }

    /// <summary>
    /// QuestDialogueDB 내 전체 데이터의 개수를 반환 
    /// </summary>
    /// <returns></returns>
    public int GetMaxCount()
    {
        return questDialogueDB.Count;
    }
}
