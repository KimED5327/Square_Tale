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
            if (_isActivate) return;

            _isActivate = true;
            _tfTarget = other.transform;
            _destPos = _tfTarget.position + Vector3.down * _downOffset;
            StartCoroutine(SandTrapActive());
        }
    }

    IEnumerator SandTrapActive()
    {
        PlayerMove player = _tfTarget.GetComponent<PlayerMove>();
        BoxCollider colFace = player.GetFaceCol();
        BoxCollider colMyBody = player.GetComponent<BoxCollider>();
        Rigidbody rigidPlayer = player.GetComponent<Rigidbody>();

        PlayerMove.s_canMove = false;

        player.enabled = false;
        colFace.enabled = false;
        colMyBody.enabled = false;
        rigidPlayer.useGravity = false;

        while (true)
        {
            if (Vector3.SqrMagnitude(_tfTarget.position - _destPos) <= 1f)
                break;

            rigidPlayer.velocity = Vector3.zero;    
            _tfTarget.position = Vector3.MoveTowards(_tfTarget.position, _destPos, _downSpeed * Time.deltaTime);
            yield return null;
        }


        rigidPlayer.useGravity = true;

        yield return new WaitForSeconds(2.5f);


        player.enabled = true;
        colFace.enabled = true;
        colMyBody.enabled = true;


        _isActivate = false;
    }
}
