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

    // 퀘스트 다이얼로그 추가 
    public void AddDialogue(int questId, QuestDialogue questDialogue)
    {
        questDialogueDB.Add(questId, questDialogue);
    }

    // 퀘스트ID에 맞는 퀘스트 다이얼로그 반환 
    public QuestDialogue GetDialogue(int questId)
    {
        QuestDialogue questDialouge = questDialogueDB[questId];

        if (questDialouge == null) Debug.Log("퀘스트 ID " + questId + "번은 등록되어있지 않습니다.");

        return questDialouge;
    }

    // 해당 퀘스트ID를 key로 가진 데이터가 있는지 확인하여 bool 타입으로 반환  
    public bool CheckKey(int questId)
    {
        return questDialogueDB.ContainsKey(questId);
    }

    // 퀘스트 다이얼로그 최대개수 반환 
    public int GetMaxCount()
    {
        return questDialogueDB.Count;
    }
}
