﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] Text _txtTitle;    // 슬롯란의 퀘스트 제목 
    QuestMenu _questMenu;               // 퀘스트 메뉴 참조값 
    Quest _quest;                       // 퀘스트 참조값 

    private void Start()
    {
        _questMenu = FindObjectOfType<QuestMenu>();
    }

    public void SelectSlot()
    {
        _questMenu.SetQuestInfo(_quest);
    }

    //getter 
    public Text GetTitle() { return _txtTitle; }
    public Quest GetQuest() { return _quest; }

    //setter 
    public void SetTitle(string title) { _txtTitle.text = title; }
    public void SetQuest(Quest quest) { _quest = quest; }
}
