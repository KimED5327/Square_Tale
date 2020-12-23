using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{

    Enemy _enemy;
    public Slider hpBar;
    Transform cam;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        cam = Camera.main.transform;
        hpBar = GetComponentInChildren<Slider>();

    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpBar.value = _enemy.currentHp / (float)_enemy.maxHp;
    }
}
