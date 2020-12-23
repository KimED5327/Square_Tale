﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    public Transform child;

    float hAxis;
    float vAxis;

    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody myRigid;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Dodge();
    }

    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        Vector3 realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);

        realMoveVec.y = 0;

        if (isDodge)
        {
            moveVec = dodgeVec;
        }

        transform.position += realMoveVec * speed * Time.deltaTime;

        child.LookAt(child.position + realMoveVec);

        anim.SetBool("isRun", realMoveVec != Vector3.zero);
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && !isJump && !isDodge)
        {
            myRigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (Input.GetKeyDown("left shift") && !isJump && moveVec != Vector3.zero && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
