using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWithLines : NPC
{
    List<int> _questList = new List<int>();       // NPC에게 할당된 퀘스트 ID 리스트 
    List<string> _lineList = new List<string>();  // NPC 기본 대사 리스트 

    //getter
    public List<string> GetLineList() { return _lineList; }
    public List<int> GetQuestList() { return _questList; }
    public int GetLinesCount() { return _lineList.Count; }
    public int GetQuestsCount() { return _questList.Count; }

    /// <summary>
    /// string 리스트(대사 리스트)에서 idx 순서의 데이터 반환. lineList[idx]
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public string GetLine(int idx) { return _lineList[idx]; }

    /// <summary>
    /// int 리스트(퀘스트 ID 리스트)에서 idx 순서의 데이터 반환. questList(idx)
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public int GetQuestID(int idx) { return _questList[idx]; }

    //setter
    public void SetLineList(List<string> lineList) { _lineList = lineList; }
    public void SetQuestList(List<int> questList) { _questList = questList; }

    /// <summary>
    /// string 리스트(대사 리스트)에 line을 새 원소로 추가 
    /// </summary>
    /// <param name="line"></param>
    public void AddLine(string line) { _lineList.Add(line); }

    /// <summary>
    /// int 리스트(퀘스트 ID 리스트)에 questID를 새 원소로 추가 
    /// </summary>
    /// <param name="line"></param>
    public void AddQuestID(int questID) { _questList.Add(questID); }
}
