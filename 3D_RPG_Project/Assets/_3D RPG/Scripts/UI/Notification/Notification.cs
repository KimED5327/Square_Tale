using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    [Header("Slide")]
    [SerializeField] GameObject _goPopup = null;
    [SerializeField] Text _txtSlideMain = null;
    [SerializeField] Text _txtSlideSub = null;

    [Header("Floating")]
    [SerializeField] Text _txtFloatingText = null;

    [Header("Keyword")]
    [SerializeField] Text _txtKeyword = null;
    [SerializeField] GameObject _goAll = null;

    Animator _anim;
    string _aniPlay = "Play";
    string _aniKeyword = "Keyword";
    string _aniSlide = "Slide";

    private void Awake()
    {
        instance = this;
        _anim = GetComponent<Animator>();
    }
    

    public void ShowKeywordText(string msg, string keyword)
    {
        _goAll.SetActive(false);
        _txtKeyword.gameObject.SetActive(true);

        string message = msg;
        message = message.Replace("keyword", keyword);
        _txtKeyword.text = message;
        ScreenEffect.instance.ExecuteSplash(0.5f);
        _anim.SetTrigger(_aniKeyword);
    }

    public void ShowBlockText()
    {
        _goAll.SetActive(false);
        _txtKeyword.gameObject.SetActive(true);

        string message = StringManager.msgBlockAcquire;
        _txtKeyword.text = message;
        ScreenEffect.instance.ExecuteSplash(0.5f);
        _anim.SetTrigger(_aniKeyword);
    }


    public void ShowAllItemNotice(int count)
    {
        _goAll.SetActive(true);
        _txtKeyword.gameObject.SetActive(false);

        string message = $"{count} 골드 획득!!";
        _txtKeyword.text = message;
        ScreenEffect.instance.ExecuteSplash(0.5f);
        _anim.SetTrigger(_aniKeyword);
    }



    public void ShowMsg(string msg)
    {
        _goAll.SetActive(false);
        _txtKeyword.gameObject.SetActive(true);

        _txtKeyword.text = msg;
        ScreenEffect.instance.ExecuteSplash(0.5f);
        _anim.SetTrigger(_aniKeyword);
    }

    public void ShowFloatingMessage(string msg)
    {
        _txtFloatingText.text = msg;
        _anim.SetTrigger(_aniPlay);
    }

    public void ShowSlideMessage(string msg)
    {
        _txtSlideMain.text = StringManager.GetMapKoreanName(msg);
        _txtSlideSub.text = msg;
        _goPopup.SetActive(true);
        _anim.SetTrigger(_aniSlide);
    }

    public void BtnHidePopup()
    {
        _goPopup.SetActive(false);
    }
}
