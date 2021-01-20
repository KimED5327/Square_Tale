using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] GameObject _goUI = null;

    [SerializeField] Slider _sliderBGM = null;
    [SerializeField] Slider _sliderSFX = null;

    public void BtnOptionOpen()
    {
        SoundManager.instance.PlayEffectSound("PopUp");
        _goUI.SetActive(true);
    }

    public void BtnBack()
    {

        SoundManager.instance.PlayEffectSound("PopDown");
        _goUI.SetActive(false);
    }

    public void BtnExit()
    {

        SoundManager.instance.PlayEffectSound("Click");
        Application.Quit();
    }

    public void BtnLobby()
    {

        SoundManager.instance.PlayEffectSound("Click");
        LoadingScene.LoadScene("LobbyScene");
    }

    public void OnBgmHandleChanged()
    {
        SoundManager.instance.SetBgmVolumn(_sliderBGM.value);
    }


    public void OnSfxHandleChanged()
    {
        SoundManager.instance.SetSfxVolumn(_sliderSFX.value);
    }


}
