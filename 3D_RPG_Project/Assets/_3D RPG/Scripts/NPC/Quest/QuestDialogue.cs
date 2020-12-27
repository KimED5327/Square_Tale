using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 다이얼로그 
// NPC의 디폴트 대사는 퀘스트 대사와 공유하는 변수가 다르고,
// 같은 시스템 내에 있을 경우 조건 검사가 더 까다로워지므로 NPC 데이터베이스에 저장 
[System.Serializable]
public class QuestDialogue 
{
    int questId;                                                // 퀘스트 ID
    int npcId;                                                  // NPC ID
    
    // 리스트로 선언해도 되지만 퀘스트 진행상태에 따른 대화의 개수가 명확히 정해져있으므로 
    // 동적할당이 필요하지 않기 때문에 불필요하게 추가적인 메모리를 사용하지 않는 정적배열로 선언 
    DialogueUnit[] dialoguePerState = new DialogueUnit[3];      // 퀘스트 진행상태에 따른 다이얼로그
 
    //getter 
    public int GetQuestId() { return questId; }
    public int GetNPCId() { return npcId; }
    public DialogueUnit GetDialogue(QuestState state)
    { return dialoguePerState[(int)state]; }

    //setter
    public void SetQuestId(int id) { questId = id; }
    public void SetNPCId(int id) { npcId = id; }
    public void SetDialogue(DialogueUnit dialogue, QuestState state)
    { dialoguePerState[(int)state] = dialogue; }
}

// 대화 단위 클래스 
[System.Serializable]
public class DialogueUnit
{
    QuestState questState;      // 퀘스트 진행상태  
    List<LineUnit> lineLists;   // 퀘스트 진행상태에 따른 대사 리스트 

    // getter 
    public QuestState GetQuestState() { return questState; }
    public List<LineUnit> GetLineLists() { return lineLists; }

    // setter 
    public void SetQuestState(QuestState state) { questState = state; }
    public void SetLineLists(List<LineUnit> lists) { lineLists = lists; }
}

// 대사 단위 클래스 
[System.Serializable]
public class LineUnit
{
    int lineId;     // 대사 ID
    string line;    // 대사 (화면에 한번에 출력되는 단위)

    // getter
    public int GetLineId() { return lineId; }
    public string GetLine() { return line; }

    // setter 
    public void SetLineId(int id) { lineId = id; }
    public void SetLine(string str) { line = str; }
}
