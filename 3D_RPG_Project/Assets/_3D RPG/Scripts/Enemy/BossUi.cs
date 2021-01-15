using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossUi : MonoBehaviour
{

    public GameObject _boss;
    EnemyStatus _BossStatus;
    public Slider hpBar;
    public Text _Ui;
    float hpPercent;

    void Start()
    {

        _BossStatus = _boss.GetComponent<EnemyStatus>();
        hpBar = GetComponentInChildren<Slider>();
    }

    void Update()
    {

        hpPercent = (_BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp()) * 100;

        if (hpBar == null)
        {
            hpBar = GetComponentInChildren<Slider>();
        }


        hpBar.value = _BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp();
    }
}
