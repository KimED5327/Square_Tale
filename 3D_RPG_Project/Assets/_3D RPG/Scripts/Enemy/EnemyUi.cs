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
    public Text _Ui2;
    float hpPercent;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemyStatus = GetComponentInParent<EnemyStatus>();
        cam = Camera.main.transform;
        hpBar = GetComponentInChildren<Slider>();
        
    }

    private void Update()
    {
        hpPercent = (_enemyStatus.GetCurrentHp() / (float)_enemyStatus.GetMaxHp()) * 100;
        if (_enemy.enemyState == Enemy.State.Attack || _enemy.enemyState == Enemy.State.Move)
        {
            hpBar.gameObject.SetActive(true);
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpBar.value = _enemyStatus.GetCurrentHp() / (float)_enemyStatus.GetMaxHp();
        _Ui.text = "Lv " + _enemyStatus.GetLevel() + "몬스터";
        _Ui2.text = _enemyStatus.GetName() + hpPercent + "%";
    }
}
