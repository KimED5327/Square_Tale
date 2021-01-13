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

    [SerializeField] float _alpha = 0.15f;

    int _combo = 0;

    void Start()
    {
        _status = GetComponentInParent<PlayerStatus>();
        _enableWait = new WaitForSeconds(enableTime);
        _disableWait = new WaitForSeconds(disableTime);
    }

    public void Use(int idx)
    {
        _combo = idx;
        StopAllCoroutines();
        switch(idx)
        {
            case 1:
                StartCoroutine(Swing());
                break;
            case 2:
                StartCoroutine(Swing());
                break;
            case 3:
                StartCoroutine(LastSwing());
                break;
        }
    }

    IEnumerator Swing()
    {
        attackArea.enabled = false;

        yield return _enableWait;
        if (_combo == 2)
            yield return new WaitForSeconds(0.25f);
        else
            yield return new WaitForSeconds(0.05f);

        attackArea.enabled = true;

        yield return _disableWait;

        attackArea.enabled = false;
    }

    IEnumerator LastSwing()
    {
        attackArea.enabled = false;

        //yield return _enableWait;
        yield return new WaitForSeconds(0.5f);
        attackArea.enabled = true;

        yield return _disableWait;
        //yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.enemyTag) || other.CompareTag(StringManager.blockTag))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                if(_combo != 3)
                    SoundManager.instance.PlayEffectSound("Slash1");
                else
                    SoundManager.instance.PlayEffectSound("Slash2");

                ScreenEffect.instance.ExecuteSplash(_alpha);

                other.GetComponent<Status>().Damage((int)(_status.GetStr() * 0.5f), transform.position);
            }
        }
    }

    public float GetWeaponRate() { return rate; }
}
