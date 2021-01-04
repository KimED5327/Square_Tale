using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTest : MonoBehaviour
{
    Dictionary<int, string[]> _talkData;

    private void Awake()
    {
        _talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()
    {
        _talkData.Add(1, new string[] { "안녕?", "이 곳에 처음 왔구나?" });
        _talkData.Add(2, new string[] { "평범한 나무상자다." });
        _talkData.Add(3, new string[] { "누군가 사용한 흔적이 있는 책상이다." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == _talkData[id].Length) return null;
        else return _talkData[id][talkIndex];
    }
}
