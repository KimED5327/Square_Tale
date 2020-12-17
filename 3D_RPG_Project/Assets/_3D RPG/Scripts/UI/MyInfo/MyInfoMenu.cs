using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoMenu : MonoBehaviour
{
    [SerializeField] GameObject goMyInfoMenu = null;
    [SerializeField] Text txtMyName = null;
    [SerializeField] Text txtLevel = null;
    [SerializeField] Text txtStr = null;
    [SerializeField] Text txtDex = null;
    [SerializeField] Text txtDef = null;
    [SerializeField] Text txtHp = null;
    [SerializeField] Text txtMp = null;
    [SerializeField] Image imgUser = null;

    bool isOpen = false;

    // PlayerStatus thePlayerStatus;

    void Awake()
    {
        //thePlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    public void OnTouchMenu()
    {
        isOpen = !isOpen;

        if (isOpen) ShowMenu();
        else HideMenu();
    }
    
    void ShowMenu() {
        goMyInfoMenu.SetActive(true);
    }

    public void HideMenu()
    {
        goMyInfoMenu.SetActive(false);
    }
}
