using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 다이얼로그 
// NPC의 디폴트 대사는 퀘스트 대사와 공유하는 변수가 다르고,
// 같은 시스템 내에 있을 경우 조건 검사가 더 까다로워지므로 NPC 데이터베이스에 저장 
[System.Serializable]
public class QuestDialogue 
{
    int _questId;                                                // 퀘스트 ID
    int _npcId;                                                  // NPC ID
    
    // 리스트로 선언해도 되지만 퀘스트 진행상태에 따른 대화의 개수가 명확히 정해져있으므로 
    // 동적할당이 필요하지 않기 때문에 불필요하게 추가적인 메모리를 사용하지 않도록 정적배열로 선언 
    DialogueUnit[] _dialoguePerState = new DialogueUnit[3];      // 퀘스트 진행상태에 따른 다이얼로그
 
    //getter 
    public int GetQuestId() { return _questId; }
    public int GetNPCId() { return _npcId; }
    public DialogueUnit GetDialoguePerState(QuestState state) { return _dialoguePerState[(int)state - 1]; }
    public int GetLinesCount(QuestState state) { return _dialoguePerState[(int)state - 1].GetLineLists().Count; }
    public string GetLine(QuestState state, int idx) { return _dialoguePerState[(int)state - 1].GetLineLists()[idx]; }

    //setter
    public void SetQuestId(int id) { _questId = id; }
    public void SetNPCId(int id) { _npcId = id; }
    public void SetDialoguePerState(DialogueUnit dialogue, QuestState state) { _dialoguePerState[(int)state - 1] = dialogue; }
}

// 대화 단위 클래스 
[System.Serializable]
public class DialogueUnit
{
    int _lineId;                 // 대사 ID
    QuestState _questState;      // 퀘스트 진행상태  
    List<string> _lineLists;     // 퀘스트 진행상태에 따른 대사 리스트

    // getter 
    public int GetLineId() { return _lineId; }
    public QuestState GetQuestState() { return _questState; }
    public List<string> GetLineLists() { return _lineLists; }

    // setter 
    public void SetLineId(int id) { _lineId = id; }
    public void SetQuestState(QuestState state) { _questState = state; }
    public void SetLineLists(List<string> lists) { _lineLists = lists; }
    public void AddLine(string line) { _lineLists.Add(line); }
}

