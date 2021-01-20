using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    Transform player;
    EnemyStatus states;
    Boss boss;
    bool isSkiil = false;

    float _curTime = 0f;
    float _termTime = 0.15f;

    public void Pushinfo(Transform transform, EnemyStatus enemystatus, Boss bo)
    {
        player = transform;
        states = enemystatus;
        boss = bo;
    }
    void Start()
    {
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
        //Destroy(gameObject,5);
	}

    void FixedUpdate ()
    {
		if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    
	}

    private void Update()
    {
        if (boss == null) return;
        if (transform.CompareTag("BossSkill") && boss.getIsDamage() && !isSkiil)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            isSkiil = true;
        }
        if (!boss.getIsDamage())
        {
            isSkiil = false;
        }
    }

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _curTime -= Time.deltaTime;
            if(_curTime <= 0)
            {
                other.transform.GetComponent<Status>().Damage(10, transform.position);
                _curTime = _termTime;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _curTime = 0f;
    }

    void OnCollisionEnter(Collision collision)
    {

        //Lock all axes movement and rotation

        if (collision.transform.CompareTag("Enemy")) return;
       

        if(transform.CompareTag("BossAttack"))
        {
           if(collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Status>().Damage(states.GetAtk(), transform.position);
            }
            Destroy(gameObject);
        }
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        
    }
}
