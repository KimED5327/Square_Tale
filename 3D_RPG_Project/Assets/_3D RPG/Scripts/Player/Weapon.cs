using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float rate;
    public BoxCollider attackArea;

    [SerializeField] float enableTime = 0.25f;
    [SerializeField] float disableTime = 0.5f;

    WaitForSeconds enableWait;
    WaitForSeconds disableWait;

    void Start()
    {
        enableWait = new WaitForSeconds(enableTime);
        disableWait = new WaitForSeconds(disableTime);
    }

    public void Use()
    {
        StopAllCoroutines();
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        attackArea.enabled = false;

        yield return enableWait;
        attackArea.enabled = true;

        yield return disableWait;
        attackArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                other.GetComponent<Status>().Damage(50, transform.position);
            }
        }
    }
}
