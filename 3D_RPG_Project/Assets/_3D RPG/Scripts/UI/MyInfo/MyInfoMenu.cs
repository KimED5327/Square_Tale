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
        
        if (player.GetIsSword())
        {
            txtAtkType.text = "STR";
            txtAtkValue.text = thePlayerStatus.GetStr().ToString();
            //txtDef.text = thePlayerStatus.GetSwordDef().ToString();
            //txtHp.text = thePlayerStatus.GetSwordMaxHp().ToString();
        }
        if (player.GetIsMage())
        {
            txtAtkType.text = "INT";
            txtAtkValue.text = thePlayerStatus.GetInt().ToString();
            //txtDef.text = thePlayerStatus.GetMageDef().ToString();
            //txtHp.text = thePlayerStatus.GetMageMaxHp().ToString();
        }
        txtHp.text = thePlayerStatus.GetMaxHp().ToString();
        txtDef.text = thePlayerStatus.GetDef().ToString();
        
        imgUser.sprite = null;

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
        if (!player.GetIsSkill1() && !player.GetIsSkill2() && !player.GetIsSkill3() && !player.GetIsSkill4())
        {
            SoundManager.instance.PlayEffectSound("Click");
            player.anim = player.anims[0];
            sword.SetActive(true);
            mage.SetActive(false);
            player.SetIsSword(true);
            player.SetIsMage(false);
            ShowMenu();
        }
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSwap);
        }
    }

    public void SwapMage()
    {
        if (!player.GetIsSkill1() && !player.GetIsSkill2() && !player.GetIsSkill3() && !player.GetIsSkill4())
        {
            SoundManager.instance.PlayEffectSound("Click");
            player.anim = player.anims[1];
            sword.SetActive(false);
            mage.SetActive(true);
            player.SetIsSword(false);
            player.SetIsMage(true);
            ShowMenu();
        }
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSwap);
        }
    }
}
