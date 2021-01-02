using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] InputField _inNickname = null;
    [SerializeField] GameObject _goNickname = null;
    [SerializeField] GameObject _goTouch = null;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void BtnTouchScreen()
    {
        _goTouch.SetActive(false);
        _goNickname.SetActive(true);
    }

    public void BtnTouchWorld()
    {
        if(_inNickname.text == "")
        {
            Debug.Log("닉네임을 입력해주세요");
            return;
        }
        else if(_inNickname.text.Length > 16)
        {
            Debug.Log("적절한 길이의 닉네임으로 설정해주세요");
            return;
        }

        LoadingScene.LoadScene("LobbyScene");
    }


    public void BtnTouchExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}
