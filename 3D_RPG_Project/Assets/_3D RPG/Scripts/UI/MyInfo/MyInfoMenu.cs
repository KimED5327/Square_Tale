using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoMenu : MonoBehaviour
{
    [SerializeField] GameObject goMyInfoMenu = null;
    [SerializeField] Text txtMyName = null;
    [SerializeField] Text txtAtkType = null;
    [SerializeField] Text txtAtkValue = null;
    [SerializeField] Text txtDef = null;
    [SerializeField] Text txtHp = null;
    [SerializeField] Image imgUser = null;
    [SerializeField] Sprite[] _profile = null;
    [SerializeField] Text txtAdventure = null;
    [SerializeField] Image imgAdventureGauge = null;

    public GameObject sword;
    public GameObject mage;
    public PlayerMove player;

    bool isOpen = false;

    PlayerStatus thePlayerStatus;

    void Awake()
    {
        thePlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    public void OnTouchMenu()
    {
        isOpen = !isOpen;
        SaveManager.instance.Save();
        if (isOpen) ShowMenu();
        else HideMenu();
    }
    
    void ShowMenu() {

        SoundManager.instance.PlayEffectSound("PopUp");

        if (PlayerPrefs.HasKey("Nickname"))
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() +  $" - {PlayerPrefs.GetString("Nickname")}";
        else
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() + $" - 닉네임";
        
        if (player.GetIsSword())
        {
            imgUser.sprite = _profile[0];
            txtAtkType.text = "STR";
            txtAtkValue.text = thePlayerStatus.GetStr().ToString();
        }
        if (player.GetIsMage())
        {
            imgUser.sprite = _profile[1];
            txtAtkType.text = "INT";
            txtAtkValue.text = thePlayerStatus.GetInt().ToString();
        }
        txtHp.text = thePlayerStatus.GetMaxHp().ToString();
        txtDef.text = thePlayerStatus.GetDef().ToString();

        float percent = Adventure.GetAdventureProgress();
        txtAdventure.text = percent + " %";
        imgAdventureGauge.fillAmount = percent / 100;

        goMyInfoMenu.SetActive(true);
    }

    public void HideMenu()
    {
        SoundManager.instance.PlayEffectSound("PopDown");
        goMyInfoMenu.SetActive(false);
    }

    public void SwapSword()
    {
        txtAtkType.text = "STR";
        txtAtkValue.text = thePlayerStatus.GetStr().ToString();
        imgUser.sprite = _profile[0];
        SoundManager.instance.PlayEffectSound("Click");
    }

    public void SwapMage()
    {
        txtAtkType.text = "INT";
        txtAtkValue.text = thePlayerStatus.GetInt().ToString();
        imgUser.sprite = _profile[1];
        SoundManager.instance.PlayEffectSound("Click");
    }
}
