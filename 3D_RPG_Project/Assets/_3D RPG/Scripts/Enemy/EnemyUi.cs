using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{

    Enemy _enemy;
    EnemyStatus _enemyStatus;
    public Slider hpBar;
    Transform cam;
    public Text _Ui;
    float hpPercent;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemyStatus = GetComponentInParent<EnemyStatus>();
        cam = Camera.main.transform;
        hpBar = GetComponentInChildren<Slider>();
        
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.forward);

        hpPercent = (_enemyStatus.GetCurrentHp() / (float)_enemyStatus.GetMaxHp()) * 100;

        if (_enemy.enemyState == State.Attack)
            hpBar.gameObject.SetActive(true);
        else
            hpBar.gameObject.SetActive(false);
        

        hpBar.value = _enemyStatus.GetCurrentHp() / (float)_enemyStatus.GetMaxHp();
        _Ui.text = "Lv " + _enemyStatus.GetLevel() + " " + _enemyStatus.GetName() + " " + hpPercent + "%";
    }
}
