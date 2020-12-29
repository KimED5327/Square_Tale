using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 다이얼로그 
// NPC의 디폴트 대사는 퀘스트 대사와 공유하는 변수가 다르고,
// 같은 시스템 내에 있을 경우 조건 검사가 더 까다로워지므로 NPC 데이터베이스에 저장 
[System.Serializable]
public class QuestDialogue 
{
    int _questId;         // 퀘스트 ID
    int _npcId;           // NPC ID

    //DialogueUnit[] _dialoguePerState = new DialogueUnit[3];      // 퀘스트 진행상태에 따른 다이얼로그
    Dictionary<QuestState, DialogueUnit> _dialoguePerState;
 
    //getter 
    public int GetQuestId() { return _questId; }
    public int GetNPCId() { return _npcId; }
    //public DialogueUnit GetDialoguePerState(QuestState state) { return _dialoguePerState[(int)state - 1]; }
    //public int GetLinesCount(QuestState state) { return _dialoguePerState[(int)state - 1].GetLineLists().Count; }
    //public LineUnit GetLineUnit(QuestState state, int idx) { return _dialoguePerState[(int)state - 1].GetLineLists()[idx]; }
    //public string GetLine(QuestState state, int idx) { return _dialoguePerState[(int)state - 1].GetLineLists()[idx].GetLine(); }

    public DialogueUnit GetDialoguePerState(QuestState state) { return _dialoguePerState[state]; }
    public int GetLinesCount(QuestState state) { return _dialoguePerState[state].GetLineList().Count; }
    public LineUnit GetLineUnit(QuestState state, int idx) { return _dialoguePerState[state].GetLineList()[idx]; }
    public string GetLine(QuestState state, int idx) { return _dialoguePerState[state].GetLineList()[idx].GetLine(); } 

    //setter
    public void SetQuestId(int questId) { _questId = questId; }
    public void SetNPCId(int npcId) { _npcId = npcId; }
    public void SetDialoguePerState(QuestState state, DialogueUnit dialogue) { _dialoguePerState[state] = dialogue; }
    public void AddDialogue(QuestState state, DialogueUnit dialogue) { _dialoguePerState.Add(state, dialogue); }
    public void AddLine(QuestState state, LineUnit line) { _dialoguePerState[state].AddLine(line); }
}

// 대화 단위 클래스 
[System.Serializable]
public class DialogueUnit
{
    QuestState _questState;      // 퀘스트 진행상태  
    List<LineUnit> _lineList;

    // getter 
    public QuestState GetQuestState() { return _questState; }
    public List<LineUnit> GetLineList() { return _lineList; }

    // setter 
    public void SetQuestState(QuestState state) { _questState = state; }
    public void SetLineList(List<LineUnit> list) { _lineList = list; }
    public void AddLine(LineUnit line) { _lineList.Add(line); }
}

// 대사 단위 클래스 
[System.Serializable]
public class LineUnit
{
    int _lineId;
    int _npcId;
    string _line;

    //getter
    public int GetLineId() { return _lineId; }
    public int GetNPCId() { return _npcId; }
    public string GetLine() { return _line; }

    //setter
    public void SetLineId(int id) { _lineId = id; }
    public void SetNPCId(int id) { _npcId = id; }
    public void SetLine(string line) { _line = line; }
}

