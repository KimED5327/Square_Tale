using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC 
{
    int _id;                                      // NPC ID
    string _name;                                 // NPC 이름 
    List<string> _lineList = new List<string>();  // NPC 기본 대사 리스트 

    //getter
    public int GetID() { return _id; }
    public string GetName() { return _name; }
    public List<string> GetLineList() { return _lineList; }

    /// <summary>
    /// string 리스트(대사 리스트)에서 idx 순서의 데이터 반환. lineList[idx]
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public string GetLine(int idx) { return _lineList[idx]; }

    //setter
    public void SetID(int id) { _id = id; }
    public void SetName(string name) { _name = name; }
    public void SetLineList(List<string> lineList) { _lineList = lineList; }

    /// <summary>
    /// string 리스트(대사 리스트)에 line을 새 원소로 추가 
    /// </summary>
    /// <param name="line"></param>
    public void AddLine(string line) { _lineList.Add(line); }
}
