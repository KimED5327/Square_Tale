using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    [Header("PopUp")]
    [SerializeField] GameObject _goPopup = null;
    [SerializeField] Text _txtPopup = null;

    [Header("Floating")]
    [SerializeField] Text _txtFloatingText = null;

    [Header("Keyword")]
    [SerializeField] Text _txtKeyword = null;

    Animator _anim;
    string _aniPlay = "Play";
    string _aniKeyword = "Keyword";
    private void Awake()
    {
        instance = this;
        _anim = GetComponent<Animator>();
    }
    

    public void ShowKeywordText(string msg, string keyword)
    {
        string message = msg;
        message = message.Replace("keyword", keyword);
        _txtKeyword.text = message;
        ScreenEffect.instance.ExecuteSplash(0.5f);
        _anim.SetTrigger(_aniKeyword);
    }


    public void ShowFloatingMessage(string msg)
    {
        _txtFloatingText.text = msg;
        _anim.SetTrigger(_aniPlay);
    }

    public void ShowPopUpMessage(string msg)
    {
        _txtPopup.text = msg;
        _goPopup.SetActive(true);
    }

    public void BtnHidePopup()
    {
        _goPopup.SetActive(false);
    }
}
