using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLever : Trigger
{

    public override void ActiveTrigger()
    {
        // 작동하지 않은 상태인 경우
        if (!_isActivated)
        {
            base.ActiveTrigger();

            // 트리거 발동에 따른 이펙트 연출 필요
        }

        // 이미 작동한 경우
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgAlreadyActivateTrigger);
        }

    }
}



