using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrap : MonoBehaviour
{
    Transform _tfTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            return;

            GetComponent<Rigidbody>().useGravity = true;
            _tfTarget = other.transform;
            StartCoroutine(TrapActive());
        }

    }


    IEnumerator TrapActive()
    {
        _tfTarget.GetComponent<PlayerMove>().enabled = false;
        _tfTarget.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f);
        _tfTarget.GetComponent<PlayerMove>().enabled = true;
        _tfTarget.GetComponent<Collider>().enabled = true;

    }
}


