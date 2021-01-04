using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    [SerializeField] string _name = "name";
    [SerializeField] float _destoryTime = 0.5f;

    bool _isAwake = false;

    private void OnEnable()
    {
        if(_isAwake)
            Invoke("AutoDestroy", _destoryTime);

        _isAwake = true;
    }

    void AutoDestroy()
    {
        if(!gameObject.activeSelf)
            ObjectPooling.instance.PushObjectToPool(_name, this.gameObject);
    }

}
