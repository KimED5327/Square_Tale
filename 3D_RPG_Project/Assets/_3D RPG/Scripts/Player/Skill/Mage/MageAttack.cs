using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    float gravity = 0.01f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    PlayerStatus _status;
    void Start()
    {
        _status = FindObjectOfType<PlayerStatus>();
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            Vector3 dir = transform.position;
          
            dir += transform.forward * (speed * Time.deltaTime);
            dir.y -= gravity;
            transform.position = dir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.enemyTag) || other.CompareTag(StringManager.blockTag))
        {
            Status targetStatus = other.GetComponent<Status>();
            if (!targetStatus.IsDead())
            {
                //targetStatus.Damage(_status.GetAtk(), transform.position);
                targetStatus.Damage((int)(_status.GetInt() * 1.0f), transform.position);
                GameObject _hit = Instantiate(hit, other.transform.position, other.transform.rotation);
                Rigidbody rigid = _hit.GetComponent<Rigidbody>();
                Destroy(_hit, 0.5f);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Floor"))
        {
            GameObject _hit = Instantiate(hit, gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody rigid = _hit.GetComponent<Rigidbody>();
            Destroy(_hit, 0.5f);
            Destroy(gameObject);
        }
    }
}
