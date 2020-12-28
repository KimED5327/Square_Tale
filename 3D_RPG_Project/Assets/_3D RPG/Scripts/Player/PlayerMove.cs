using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform child;

    float hAxis;
    float vAxis;
    float attackDelay;

    bool isJump;
    bool isDodge;
    bool isAttackReady = true;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 realMoveVec;

    Rigidbody myRigid;
    Animator anim;
    Weapon equipWeapon;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
        Dodge();
    }

    void Update()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < attackDelay;
    }

    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);

        realMoveVec.y = 0;

        if (isDodge)
        {
            realMoveVec = dodgeVec;
        }

        if (!isAttackReady)
        {
            realMoveVec = Vector3.zero;
            if (isDodge)
            {
                realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);
                isAttackReady = true;
            }
        }

        transform.position += realMoveVec * speed * Time.deltaTime;

        child.LookAt(child.position + realMoveVec);

        anim.SetBool("isRun", realMoveVec != Vector3.zero);
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && !isJump && !isDodge)
        {
            isJump = true;
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    void Dodge()
    {
        if (Input.GetKeyDown("left shift") && !isJump && moveVec != Vector3.zero && !isDodge)
        {
            dodgeVec = realMoveVec;
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

    public void Attack()
    {
        if (isAttackReady && !isDodge && !isJump)
        {
            equipWeapon.Use();
            anim.SetTrigger("doAttack1");
            attackDelay = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            if (isJump)
            {
                anim.SetTrigger("forceRun");
                anim.SetBool("isJump", false);
                isJump = false;
            }
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
