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
    Dictionary<QuestState, DialogueUnit> _dialoguePerState = new Dictionary<QuestState, DialogueUnit>();
 
    //getter 
    public int GetQuestId() { return _questId; }
    public DialogueUnit GetDialoguePerState(QuestState state) { return _dialoguePerState[state]; }

    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 데이터 유무를 bool 타입으로 반환 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool CheckDialogue(QuestState state) { return _dialoguePerState.ContainsKey(state); }

    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 클래스의 lineList.Count(대사 개수) 값을 반환 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public int GetLinesCount(QuestState state) { return _dialoguePerState[state].GetLineList().Count; }

    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 클래스의 lineList(대사 리스트) 반환 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public List<LineUnit> GetLineList(QuestState state) { return _dialoguePerState[state].GetLineList(); }
    
    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 클래스의 대사 리스트에서 idx 위치의 LineUnit(대사 클래스) 반환. lineList[idx]
    /// </summary>
    /// <param name="state"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public LineUnit GetLineUnit(QuestState state, int idx) { return _dialoguePerState[state].GetLineList()[idx]; }

    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 클래스의 대사 리스트에서 idx 위치의 line(대사) 반환. lineList[idx].line 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public string GetLine(QuestState state, int idx) { return _dialoguePerState[state].GetLineList()[idx].GetLine(); } 

    //setter
    public void SetQuestId(int questId) { _questId = questId; }
    public void SetDialoguePerState(QuestState state, DialogueUnit dialogue) { _dialoguePerState[state] = dialogue; }

    /// <summary>
    /// 해당 state key, DialogueUnit value를 가진 데이터를 Dictionary에 추가 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="dialogue"></param>
    public void AddDialogue(QuestState state, DialogueUnit dialogue) { _dialoguePerState.Add(state, dialogue); }

    /// <summary>
    /// 해당 state key를 가진 DialogueUnit 클래스의 대사 리스트에 line 추가 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="line"></param>
    public void AddLine(QuestState state, LineUnit line) { _dialoguePerState[state].AddLine(line); }
}

// 대화 단위 클래스 
[System.Serializable]
public class DialogueUnit
{
    QuestState _questState;     // 퀘스트 진행상태   
    List<LineUnit> _lineList;   // 대사 리스트 

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
    int _lineId;        // 대사 ID
    int _npcId;         // NPC ID (현재 화자가 유저인지 NPC인지 확인)
    string _line;       // 대사 

    //getter
    public int GetLineId() { return _lineId; }
    public int GetNPCId() { return _npcId; }
    public string GetLine() { return _line; }

    //setter
    public void SetLineId(int lineId) { _lineId = lineId; }
    public void SetNPCId(int npcId) { _npcId = npcId; }
    public void SetLine(string line) { _line = line; }
}

