using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHudMenu : MonoBehaviour
{
    public static GameHudMenu instance;

    public static bool s_isMenuOpen = false;

    [SerializeField] GameObject[] goHuds = null;
    [SerializeField] Text _txtGold = null;
    [SerializeField] Text _txtLevel = null;
    [SerializeField] Image _imgExp = null;
    [SerializeField] Image _imgHp = null;

    WaitForSeconds waitTime = new WaitForSeconds(0.1f);

    Inventory _inven;
    PlayerStatus _playerStatus;

    private void Start()
    {
        _inven = FindObjectOfType<Inventory>();
        _playerStatus = FindObjectOfType<PlayerStatus>();

        _txtGold.text = string.Format("{0:#,##0}", _inven.GetGold());

        instance = this;

        //StartCoroutine(UpdateHud());
    }

    void Update()
    {
        if (_playerStatus != null)
        {

            _txtGold.text = string.Format("{0:#,##0}", _inven.GetGold());
            _txtLevel.text = $"{_playerStatus.GetLevel()}";
            _imgExp.fillAmount = _playerStatus.GetExpPercent();
            _imgHp.fillAmount = _playerStatus.GetHpPercent();
            
            //_txtHP.text = $"{_playerStatus.GetCurrentHp()} / {_playerStatus.GetMaxHp()}";
            //_txtMP.text = $"{_playerStatus.GetCurMp()} / {_playerStatus.GetMaxMp()}";
        }

    }

    public void HideMenu()
    {
        s_isMenuOpen = true;
        for (int i = 0; i < goHuds.Length; i++)
        {
            goHuds[i].SetActive(false);
        }
    }

    public void ShowMenu()
    {

        s_isMenuOpen = false;
        for (int i = 0; i < goHuds.Length; i++)
        {
            goHuds[i].SetActive(true);
        }

    }

}
