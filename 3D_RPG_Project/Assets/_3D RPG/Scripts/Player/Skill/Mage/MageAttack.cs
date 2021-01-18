using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public float gravity = 0.02f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    PlayerStatus _status;
    int _combo = 0;

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

    public void SetDir(Vector3 dir)
    {
        transform.forward = dir;
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
                switch(_combo)
                {
                    case 1:
                        targetStatus.Damage((int)(_status.GetInt() * 1.0f), transform.position);
                        GameObject _hit1 = Instantiate(hit, other.transform.position, other.transform.rotation);
                        Destroy(_hit1, 0.5f);
                        Destroy(gameObject);
                        break;
                    case 2:
                        targetStatus.Damage((int)(_status.GetInt() * 0.7f), transform.position);
                        GameObject _hit2 = Instantiate(hit, other.transform.position, other.transform.rotation);
                        Destroy(_hit2, 0.5f);
                        Destroy(gameObject);
                        break;
                    case 3:
                        targetStatus.Damage((int)(_status.GetInt() * 0.6f), transform.position);
                        GameObject _hit3 = Instantiate(hit, other.transform.position, other.transform.rotation);
                        Destroy(_hit3, 0.5f);
                        Destroy(gameObject);
                        break;
                }
            }
        }
        if (other.CompareTag("Floor"))
        {
            GameObject _hit = Instantiate(hit, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(_hit, 0.5f);
            Destroy(gameObject);
        }
    }

    public void setCombo(int combo) { _combo = combo; } 
}
