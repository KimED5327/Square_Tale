using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTrap : MonoBehaviour
{
    [SerializeField] float _downSpeed = 0.25f;
    [SerializeField] float _downOffset = 1f;
    Transform _tfTarget;
    Vector3 _destPos;
    bool _isActivate = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {

            return;

            if (_isActivate) return;

            _isActivate = true;
            _tfTarget = other.transform;
            _destPos = _tfTarget.position + Vector3.down * _downOffset;
            StartCoroutine(SandTrapActive());
        }
    }

    IEnumerator SandTrapActive()
    {
        _tfTarget.GetComponent<PlayerMove>().enabled = false;
        _tfTarget.GetComponent<BoxCollider>().enabled = false;
        _tfTarget.GetComponent<Rigidbody>().useGravity = false;

        while (true)
        {
            if (Vector3.SqrMagnitude(_tfTarget.position - _destPos) <= 0.1f)
                break;
            _tfTarget.position = Vector3.MoveTowards(_tfTarget.position, _destPos, _downSpeed * Time.deltaTime);
            yield return null;
        }

        _tfTarget.GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(0.5f);
        _tfTarget.GetComponent<PlayerMove>().enabled = true;
        _tfTarget.GetComponent<BoxCollider>().enabled = true;


        _isActivate = false;
    }
}
