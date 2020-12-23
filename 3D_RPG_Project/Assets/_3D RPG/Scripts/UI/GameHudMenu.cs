using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHudMenu : MonoBehaviour
{
    public static GameHudMenu instance;
    
    [SerializeField] GameObject[] goHuds = null;
    [SerializeField] Text _txtGold = null;


    Inventory theInven;

    private void Awake()
    {
        theInven = FindObjectOfType<Inventory>();
        _txtGold.text = string.Format("{0:#,##0}", theInven.GetGold());

        instance = this;
    }
    public void HideMenu()
    {
        for (int i = 0; i < goHuds.Length; i++)
        {
            goHuds[i].SetActive(false);
        }
    }
    public void ShowMenu()
    {
        for (int i = 0; i < goHuds.Length; i++)
        {
            goHuds[i].SetActive(true);
        }

        _txtGold.text = string.Format("{0:#,##0}", theInven.GetGold());
    }

}
