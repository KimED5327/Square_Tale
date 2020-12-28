using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{

    Enemy _enemy;
    public Slider hpBar;
    Transform cam;
    public Text _Ui;
    public Text _Ui2;
    int hpPercent;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        cam = Camera.main.transform;
        hpBar = GetComponentInChildren<Slider>();
        
    }

    private void Update()
    {
        hpPercent = (_enemy.currentHp / _enemy.maxHp * 100);
        if (_enemy.enemyState == Enemy.State.Attack || _enemy.enemyState == Enemy.State.Move)
        {
            hpBar.gameObject.SetActive(true);
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpBar.value = _enemy.currentHp / (float)_enemy.maxHp;
        _Ui.text = "Lv " + _enemy.level + "몬스터";
        _Ui2.text = _enemy.name + hpPercent + "%";
    }
}
