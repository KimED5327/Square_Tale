﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Adventure : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _goUI = null;
    [SerializeField] Text _txtSynopsis = null;
    [SerializeField] Text _txtPage = null;

    [Header("Slots")]
    [SerializeField] KeywordSlot[] _slots = null;
    [SerializeField] string _printText = "??????";

    int _curChapter = 0;
    int _curPage = 0;
    int _maxPage = 1;
    int _pageKeywordMaxCount;

    int _startKeywordID = 0;

    // 모험담 메뉴출력
    public void ShowUI()
    {
        GameHudMenu.instance.HideMenu();

        _curChapter = 0;
        _curPage = 0;
        KeywordSetting();

        _goUI.SetActive(true);
    }

    public void HideUI()
    {
        GameHudMenu.instance.ShowMenu();

        _goUI.SetActive(false);
    }

    // 키워드 슬롯 세팅
    void KeywordSetting()
    {
        _startKeywordID = KeywordData.instance.GetStartKeywordID(_curChapter, _curPage);
        _pageKeywordMaxCount = KeywordData.instance.GetKeywordMaxCount(_curChapter, _curPage);

        // 현재 챕터의 현재 페이지 키워드 세팅
        for (int i = 0; i < _slots.Length; i++)
        {
            Keyword keyword = (i < _pageKeywordMaxCount) 
                            ? KeywordData.instance.GetKeyword(_startKeywordID + i)
                            : null;

            if (keyword != null)
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].SetSlot(keyword, _printText);
            }
            else
                _slots[i].gameObject.SetActive(false);

        }

        // 시놉시스 세팅
        _txtSynopsis.text = KeywordData.instance.GetSynopsis(_curChapter, _curPage);

        // 획득하지 않은 키워드는 시놉시스에서 특수문자로 대체
        List<Keyword> curChapterKeywordList = KeywordData.instance.GetCurCpKeywordList(_curChapter);
        for (int i = 0; i < curChapterKeywordList.Count; i++)
        {
            if (!curChapterKeywordList[i].isGet)
                _txtSynopsis.text = _txtSynopsis.text.Replace(curChapterKeywordList[i].keyword, curChapterKeywordList[i].hideText);
        }

        _txtPage.text = (_curPage + 1).ToString();
    }

    // 챕터 선택
    public void BtnSelectChapter(int index)
    {
        _curChapter = index;
        _curPage = 0;

        KeywordSetting();
    }

    // 페이지 이동
    public void BtnMovePage(int count)
    {
        _curPage = (_curPage + count) % (_maxPage + 1);

        KeywordSetting();
    }

}