using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float rate = 0.7f;
    [SerializeField] BoxCollider attackArea = null;

    [SerializeField] float enableTime = 0.25f;
    [SerializeField] float disableTime = 0.5f;

    WaitForSeconds _enableWait;
    WaitForSeconds _disableWait;

    PlayerStatus _status;

    void Start()
    {
        _status = GetComponentInParent<PlayerStatus>();
        _enableWait = new WaitForSeconds(enableTime);
        _disableWait = new WaitForSeconds(disableTime);
    }

    public void Use()
    {
        StopAllCoroutines();
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        attackArea.enabled = false;

        yield return _enableWait;
        attackArea.enabled = true;

        yield return _disableWait;
        attackArea.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                //other.GetComponent<Status>().Damage(_status.GetAtk(), transform.position);
                other.GetComponent<Status>().Damage(10, transform.position);
            }
        }
    }

    public float GetWeaponRate() { return rate; }
}
