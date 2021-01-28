using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossUi : MonoBehaviour
{

    Boss _boss;
    EnemyStatus _BossStatus;
    public Slider hpBar;
    public Text _Ui;
    float hpPercent;

    public void DeactiveUI()
    {
        hpBar.gameObject.SetActive(false);
        _Ui.gameObject.SetActive(false);
    }
    public void ActiveUI()
    {
        hpBar.gameObject.SetActive(true);
        _Ui.gameObject.SetActive(true);
    }

    void Start()
    {
        _boss = GetComponent<Boss>();
        _BossStatus = GetComponent<EnemyStatus>();

        _Ui.gameObject.SetActive(true);
        hpBar.gameObject.SetActive(true);
    }

    void Update()
    {
        hpPercent = (_BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp()) * 100;
        hpBar.value = (float)_BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp();

        _Ui.text = (int)hpPercent + "%" + " " + _BossStatus.GetName();


        if(_BossStatus.IsDead())
        {
            DeactiveUI();
        }
    }
}
