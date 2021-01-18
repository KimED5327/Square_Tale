using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMonster : Trigger
{
    EnemyStatus _target = null;

    WaitForSeconds waitTime = new WaitForSeconds(1f);

    public void SetTargetLink(EnemyStatus target)
    {
        _target = target;
        StartCoroutine(CheckTriggerActive());
    }

    IEnumerator CheckTriggerActive()
    {
        while (true)
        {
            yield return waitTime;
            if (_target.IsDead())
            {
                ActiveTrigger();
                break;
            }
        }
    }


}
