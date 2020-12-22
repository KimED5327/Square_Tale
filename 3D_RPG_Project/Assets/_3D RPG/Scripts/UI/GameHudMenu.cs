using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHudMenu : MonoBehaviour
{
    public static GameHudMenu instance;
    [SerializeField] GameObject[] goHuds = null;

    private void Awake() => instance = this;

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
    }

}
