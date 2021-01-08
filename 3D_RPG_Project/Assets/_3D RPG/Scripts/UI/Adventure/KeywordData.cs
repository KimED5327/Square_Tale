using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Keyword
{
    public int id;
    public bool isGet;
    public string keyword;
    public string hideText;
    public float progress;
}

[System.Serializable]
public class Synopsis
{
    [TextArea]
    public string[] page;
    public int[] keywordMaxCount;
}

public class KeywordData : MonoBehaviour
{
    Dictionary<int, Keyword> _keywordDatas = new Dictionary<int, Keyword>();

    List<Keyword> _cp1KeywordList = new List<Keyword>();
    List<Keyword> _cp2KeywordList = new List<Keyword>();
    

    [SerializeField] Synopsis[] _chapterSynopsis = null;


    public static KeywordData instance;

    void Awake()
    {
        instance = this;

        Keyword keyword = new Keyword { id = 1, isGet = false, keyword = "릴리", hideText = "$$", progress = 10, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 2, isGet = false, keyword = "시간", hideText = "**", progress = 10, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 3, isGet = false, keyword = "저주", hideText = "☆☆", progress = 10, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 4, isGet = false, keyword = "정화", hideText = "★★", progress = 10, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 5, isGet = false, keyword = "마음", hideText = "##", progress = 10, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 6, isGet = false, keyword = "꽃", hideText = "♡", progress = 12.5f, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 7, isGet = false, keyword = "말라가게", hideText = "&&&&", progress = 12.5f, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 8, isGet = false, keyword = "향기", hideText = "\\\\", progress = 12.5f, };
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword { id = 9, isGet = false, keyword = "독한 호흡", hideText = "++ ++", progress = 12.5f, };
        _cp1KeywordList.Add(keyword);


        for(int i = 0; i < _cp1KeywordList.Count; i++)
            AddKeywordData(_cp1KeywordList[i]);
    }

    public void AddKeywordData(Keyword keyword)
    {
        _keywordDatas.Add(keyword.id, keyword);
    }

    public void AcquireKeyword(int id)
    {
        if (_keywordDatas.ContainsKey(id))
        {
            //_keywordDatas[id].isGet = true;
            Keyword keyword = GetKeyword(id);
            keyword.isGet = true;
            string strKeyword = keyword.keyword;
            string msg = StringManager.msgGetKeword;
            float progress = keyword.progress;

            Adventure.IncreaseAdventureProgress(progress);
            Notification.instance.ShowKeywordText(msg, strKeyword);
        }
        else
            Debug.LogError("id = " + id + " 는 존재하지 않는 키워드 id입니다");
    }

    public List<Keyword> GetCurCpKeywordList(int chapterNum)
    {
        if (chapterNum == 0)
            return _cp1KeywordList;
        else
            return _cp2KeywordList;
    }

    public string GetSynopsis(int chapterNum, int page)
    {
        return _chapterSynopsis[chapterNum].page[page];
    }

    public int GetStartKeywordID(int chapterNum, int page)
    {
        int id = 1;
        for(int i = 0; i <= chapterNum; i++)
        {
            for(int k = 0; k < _chapterSynopsis[i].page.Length; k++)
            {
                if(chapterNum == i && page == k)
                    break;

                id += _chapterSynopsis[i].keywordMaxCount[k];
            }
        }

        return id;
    }

    public int GetKeywordMaxCount(int chapterNum, int page)
    {
        return _chapterSynopsis[chapterNum].keywordMaxCount[page];
    }

    public Keyword GetKeyword(int id)
    {
        if (_keywordDatas.ContainsKey(id))
            return _keywordDatas[id];
        else
            return null;
    }

}
