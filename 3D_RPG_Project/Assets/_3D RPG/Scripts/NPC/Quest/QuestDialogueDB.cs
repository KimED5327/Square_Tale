using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 다이얼로그 데이터베이스 
public class QuestDialogueDB : MonoBehaviour
{
    public static QuestDialogueDB instance;
    Dictionary<int, QuestDialogue> questDialogueDB = new Dictionary<int, QuestDialogue>();

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    /// <summary>
    /// questId key, QuestDialogue value를 가진 데이터를 데이터베이스 Dictionary에 추가 
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="questDialogue"></param>
    public void AddDialogue(int questId, QuestDialogue questDialogue)
    {
        questDialogueDB.Add(questId, questDialogue);
    }

    /// <summary>
    /// questId key값에 맞는 QuestDialogue 데이터를 반환 
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public QuestDialogue GetDialogue(int questId)
    {
        QuestDialogue questDialouge = questDialogueDB[questId];

        if (questDialouge == null) Debug.Log("퀘스트 ID " + questId + "번은 등록되어있지 않습니다.");

        return questDialouge;
    }

    /// <summary>
    /// 해당 questId를 key로 가진 데이터 유무를 bool 타입으로 반환 
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public bool CheckKey(int questId)
    {
        return questDialogueDB.ContainsKey(questId);
    }

    /// <summary>
    /// QuestDialogueDB 내 데이터의 개수를 반환 
    /// </summary>
    /// <returns></returns>
    public int GetMaxCount()
    {
        return questDialogueDB.Count;
    }
}
