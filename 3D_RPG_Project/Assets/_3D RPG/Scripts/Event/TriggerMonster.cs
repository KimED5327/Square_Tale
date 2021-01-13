using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMonster : Trigger
{
    [SerializeField] GameObject _goTarget = null;
    WaitForSeconds waitTime = new WaitForSeconds(1f);

    private void Start()
    {
        StartCoroutine(CheckTriggerActive());
    }


    IEnumerator CheckTriggerActive()
    {

        while (true)
        {
            yield return waitTime;
            if (!_goTarget.activeSelf)
            {
                ActiveTrigger();
                break;
            }
        }
    }


}
