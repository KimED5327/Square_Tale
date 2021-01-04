﻿using System.Collections;
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

        if(PlayerPrefs.HasKey("Nickname"))
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() +  $" - {PlayerPrefs.GetString("Nickname")}";
        else
            txtMyName.text = "LV " + thePlayerStatus.GetLevel() + $" - 닉네임";

        txtStr.text = thePlayerStatus.GetStr().ToString();
        txtInt.text = thePlayerStatus.GetInt().ToString();
        txtDef.text = thePlayerStatus.GetDef().ToString();
        txtHp.text = thePlayerStatus.GetMaxHp().ToString();
        imgUser.sprite = null;

        float percent = Adventure.GetAdventureProgress();
        txtAdventure.text = percent + " %";
        imgAdventureGauge.fillAmount = percent / 100;

        goMyInfoMenu.SetActive(true);
    }

    public void HideMenu()
    {
        goMyInfoMenu.SetActive(false);
    }
}
