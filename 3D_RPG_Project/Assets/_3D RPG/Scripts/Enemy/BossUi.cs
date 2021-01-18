﻿using System.Collections;
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

    void Start()
    {
        _boss = GetComponentInParent<Boss>();
        _BossStatus = GetComponentInParent<EnemyStatus>();
    }

    void Update()
    {
        hpPercent = (_BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp()) * 100;
        hpBar.value = (float)_BossStatus.GetCurrentHp() / (float)_BossStatus.GetMaxHp();
    }
}