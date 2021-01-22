using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] InputField _inNickname = null;
    [SerializeField] GameObject _goNickname = null;
    [SerializeField] GameObject _goTouch = null;

    [SerializeField] Text _txtWarning = null;

    Animator _anim;

    bool _canTouch = false;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        SoundManager.instance.PlayBGM("BGM_Lobby");
       

        _anim = GetComponent<Animator>();

        StartCoroutine(WaitCo());
    }

    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(4.5f);
        _goTouch.SetActive(true);
        _canTouch = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BtnTouchScreen();
        }
    }

    public void BtnTouchScreen()
    {
        if (!_canTouch) return;

        _canTouch = false;
        SoundManager.instance.PlayEffectSound("Click");

        if (PlayerPrefs.HasKey(StringManager.nickname))
            LoadingScene.LoadScene("LobbyScene");
        else
        {
            _goTouch.SetActive(false);
            _goNickname.SetActive(true);
        }
    }

    public void BtnTouchWorld()
    {
        SoundManager.instance.PlayEffectSound("Click");

        string id = _inNickname.text;

        if (id == "")
        {
            _anim.SetTrigger("Show");
            _txtWarning.text = "닉네임을 입력해주세요.";
            return;
        }
        else if(id.Length > 6)
        {
            _anim.SetTrigger("Show");
            _txtWarning.text = "닉네임은 6글자 이내로 해주세요.";
            return;
        }
        else if (id.Length < 2)
        {
            _anim.SetTrigger("Show");
            _txtWarning.text = "닉네임은 2글자 이상으로 해주세요.";
            return;
        }
        if (!IsValidStr(id)) // 닉네임에 특수문자, 초성, 띄어쓰기가 포함된 경우.
        {
            _anim.SetTrigger("Show");
            _txtWarning.text = "초성과 띄어쓰기, 특수문자는 불가능합니다.";
            return;
        }

        PlayerPrefs.SetString(StringManager.nickname, _inNickname.text);

        LoadingScene.LoadScene("LobbyScene");
    }

    bool IsValidStr(string text)
    {
        string pattern = @"^[a-zA-Z0-9가-힣]*$";
        return Regex.IsMatch(text, pattern);
    }

    public void BtnTouchExit()
    {
        SoundManager.instance.PlayEffectSound("Click");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}
