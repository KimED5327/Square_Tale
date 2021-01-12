using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoMenu : MonoBehaviour
{
    [SerializeField] GameObject goMyInfoMenu = null;
    [SerializeField] Text txtMyName = null;
    [SerializeField] Text txtStr = null;
    [SerializeField] Text txtInt = null;
    [SerializeField] Text txtDef = null;
    [SerializeField] Text txtHp = null;
    [SerializeField] Image imgUser = null;

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

        if (isOpen) ShowMenu();
        else HideMenu();
    }
    
    void ShowMenu() {

        SoundManager.instance.PlayEffectSound("PopUp");

        if (PlayerPrefs.HasKey("Nickname"))
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() +  $" - {PlayerPrefs.GetString("Nickname")}";
        else
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() + $" - 닉네임";

        txtStr.text = thePlayerStatus.GetStr().ToString();
        txtInt.text = thePlayerStatus.GetInt().ToString();
        txtDef.text = thePlayerStatus.GetDef().ToString();
        txtHp.text = thePlayerStatus.GetMaxHp().ToString();

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
        SoundManager.instance.PlayEffectSound("Click");
        player.anim = player.anims[0];
        sword.SetActive(true);
        mage.SetActive(false);
    }

    public void SwapMage()
    {
        SoundManager.instance.PlayEffectSound("Click");
        player.anim = player.anims[1];
        sword.SetActive(false);
        mage.SetActive(true);
    }
}
