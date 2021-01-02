using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    [SerializeField] string _name = "name";
    [SerializeField] float _destoryTime = 0.5f;

    private void OnEnable()
    {
        Invoke(nameof(AutoDestory), _destoryTime);
    }

    void AutoDestroy()
    {
        if(!gameObject.activeSelf)
            ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
    }

}
