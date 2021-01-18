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
            GetComponent<Rigidbody>().useGravity = true;
            _tfTarget = other.transform;
            StartCoroutine(TrapActive());
        }

    }


    IEnumerator TrapActive()
    {
        PlayerMove player = _tfTarget.GetComponent<PlayerMove>();
        BoxCollider colFace = player.GetFaceCol();
        BoxCollider colMyBody = player.GetComponent<BoxCollider>();

        player.enabled = false;
        colFace.enabled = false;
        colMyBody.enabled = false;

        yield return new WaitForSeconds(1f);

        player.enabled = true;
        colFace.enabled = true;
        colMyBody.enabled = true;
    }
}


