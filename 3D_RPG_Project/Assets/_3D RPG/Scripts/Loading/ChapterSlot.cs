using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSlot : MonoBehaviour
{
    [SerializeField] GameObject _goScreen = null;
    [SerializeField] Image _imgChapter = null;
    [SerializeField] Text _txtChapterName = null;
    [SerializeField] string _name = "";
    string _originText;
    Sprite _originSprite;

    void Start()
    {
        _originSprite = _imgChapter.sprite;
        _originText = _txtChapterName.text;
    }

    public void LockSlot(Sprite imglock)
    {
        string str = _txtChapterName.text;

        string temp = "";
        for(int i = 0; i < str.Length; i++)
        {
            if (str.Substring(i, 1) != " ")
                temp += " ";
            else
                temp += "?";
        }

        _imgChapter.sprite = imglock;
        _txtChapterName.text = temp;
        _goScreen.SetActive(true);
    }

    public void UnLockSlot()
    {
        _imgChapter.sprite = _originSprite;
        _txtChapterName.text = _originText;
        _goScreen.SetActive(false);
    }

    public string GetName() { return _name; }
    public Sprite GetSprite() { return _originSprite; }

}
