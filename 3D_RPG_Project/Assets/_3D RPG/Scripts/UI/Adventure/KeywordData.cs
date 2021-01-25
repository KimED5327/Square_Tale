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

    public Keyword(int p_id, string p_keyword, string p_hideText, float p_progress)
    {
        id = p_id;
        isGet = false;
        keyword = p_keyword;
        hideText = p_hideText;
        progress = p_progress;
    }
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

        Keyword keyword = new Keyword(1, "릴리", "__", 10f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(2, "시간", "__", 10f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(3, "저주", "__", 10f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(4, "정화", "__", 10f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(5, "마음", "__", 10f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(6, "꽃", "_", 12.5f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(7, "말라가게", "____", 12.5f);
        _cp1KeywordList.Add(keyword);

        keyword = new Keyword(8, "향기", "__", 12.5f);
       _cp1KeywordList.Add(keyword);

        keyword = new Keyword(9, "독한", "__", 12.5f);
        _cp1KeywordList.Add(keyword);

        LoadKeyword();

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
            SaveKeyword();
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


    public void SaveKeyword()
    {
        for(int i = 0; i < _cp1KeywordList.Count; i++)
        {
            string name = StringManager.GetKeywordKey(_cp1KeywordList[i].id);   
            string boolean = _cp1KeywordList[i].isGet.ToString();

            PlayerPrefs.SetString(name, boolean);
        }
    }


    public void LoadKeyword()
    {
        string name = StringManager.GetKeywordKey(_cp1KeywordList[0].id);
        if (PlayerPrefs.HasKey(name))
        {
            for(int i = 0; i < _cp1KeywordList.Count; i++)
            {
                name = StringManager.GetKeywordKey(_cp1KeywordList[i].id);
                _cp1KeywordList[i].isGet = bool.Parse(PlayerPrefs.GetString(name));
            }
        }
        else
        {
            SaveKeyword();
        }
    }
}
