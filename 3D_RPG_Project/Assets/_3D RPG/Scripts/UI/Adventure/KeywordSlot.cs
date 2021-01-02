using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordSlot : MonoBehaviour
{
    [SerializeField] Text _txtKeyword = null;

    Keyword _keyword = null;

    public void SetSlot(Keyword keyword, string str)
    {
        _keyword = keyword;
        _txtKeyword.text = (_keyword.isGet) ? _keyword.keyword : str;
    }

    public Keyword GetKeyword()
    {
        return _keyword;
    }
}
